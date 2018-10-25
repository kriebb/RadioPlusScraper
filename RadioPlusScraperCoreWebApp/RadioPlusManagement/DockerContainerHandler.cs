using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Azure.Management.ContainerInstance.Fluent;
using Microsoft.Azure.Management.ContainerInstance.Fluent.Models;
using Microsoft.Azure.Management.ContainerRegistry.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Rest.TransientFaultHandling;
using System;
using System.Linq;

namespace RadioPlusScraperWebApi
{
    public class DockerContainerHandler : IDockerContainerHandler
    {
        private readonly string resourceGroupName = "seleniumContainerRegistry";
        private readonly string containerRegistryName = "seleniumContainerRegistry";
        private readonly string containerGroupName = "selenium-chrome-standalone";
        private readonly string seleniumRepositoryName = "selenium";

        private static string _createdContainerGroupId;
        public static string ContainerSeleniumUrl { get; set; }

        public void Start(PerformContext context = null)
        {
            if (!string.IsNullOrWhiteSpace(_createdContainerGroupId))
            {
                context.WriteLine("I was expecting that the _createdContainerGroupId should be empty. It is not. Stopping trying to start a container");
                return;
            }

            _createdContainerGroupId = "not empty";

            lock (_createdContainerGroupId)
            {

                IAzure azure = GetAzureContext(context);


                RunTaskBasedContainer(azure, context);
            }
        }

        private void RunTaskBasedContainer(IAzure azure, PerformContext context)
        {

            context.WriteLine($"\nCreating container group '{containerGroupName}'");

            Region azureRegion = Region.EuropeWest;

            IRegistry azureRegistry = azure.ContainerRegistries.GetByResourceGroup(resourceGroupName, containerRegistryName);

            string privateRepoUrl = azureRegistry.LoginServerUrl + "/" + seleniumRepositoryName;

            IRegistryCredentials acrCredentials = azureRegistry.GetCredentials();
            // Create the container group

            EnsureOnlyOneContainerInstanceExists(azure,context);
            if (!TryGetContainerInstance(azure, out string randomContainerInstance, out IContainerGroup containerGroup))
            {
                if (!TryCreateRandomSeleniumContainerInstance(context,azure, azureRegion, azureRegistry, acrCredentials,
                    privateRepoUrl, out containerGroup, out randomContainerInstance))
                {
                    throw new Exception("Couldn't get or create a selenium Container");
                }
            }


            _createdContainerGroupId = containerGroup.Id;
            ContainerSeleniumUrl = "http://" + containerGroup.IPAddress + ":4444/wd/hub";

            context.WriteLine($"Logs for container '{randomContainerInstance}':");
            context.WriteLine(containerGroup.GetLogContent(randomContainerInstance));
        }

        private bool TryGetContainerInstance(IAzure azure, out string containerId, out IContainerGroup containerGroup)
        {
            System.Collections.Generic.IEnumerable<IContainerGroup> seleniumContainerGroups = azure.ContainerGroups.ListByResourceGroup("seleniumContainerRegistry");
            containerGroup = seleniumContainerGroups.FirstOrDefault();
            containerId = containerGroup?.Id;
            return containerGroup != null;
        }

        private void EnsureOnlyOneContainerInstanceExists(IAzure azure,PerformContext context)
        {
            System.Collections.Generic.IEnumerable<IContainerGroup> seleniumContainerGroups = azure.ContainerGroups.ListByResourceGroup("seleniumContainerRegistry");
            if (seleniumContainerGroups.Count() > 1)
            {
                context.WriteLine("We found multiple containers. Going to stop all, except the first in the list");
                IContainerGroup first = seleniumContainerGroups.First();
                System.Collections.Generic.List<IContainerGroup> listOfContainersToRemove = seleniumContainerGroups.ToList();
                listOfContainersToRemove.Remove(first);
                foreach (IContainerGroup container in listOfContainersToRemove)
                {
                    DoStop(azure, container.Id,context);
                }
            }

        }

        private bool TryCreateRandomSeleniumContainerInstance(PerformContext context, IAzure azure, Region azureRegion, IRegistry azureRegistry,
            IRegistryCredentials acrCredentials, string privateRepoUrl, out IContainerGroup containerGroup, out string randomContainerInstance)
        {
            try
            {
                string randomContainerGroupName = SdkContext.RandomResourceName(containerGroupName, 63);
                randomContainerInstance = SdkContext.RandomResourceName(seleniumRepositoryName + "-instance-", 63);
                containerGroup = azure.ContainerGroups.Define(randomContainerGroupName)
                    .WithRegion(azureRegion)
                    .WithExistingResourceGroup(resourceGroupName)
                    .WithLinux()
                    .WithPrivateImageRegistry(azureRegistry.LoginServerUrl, acrCredentials.Username,
                        acrCredentials.AccessKeys[AccessKeyType.Primary])
                    .WithoutVolume()
                    .DefineContainerInstance(randomContainerInstance)
                    .WithImage(privateRepoUrl)
                    .WithExternalTcpPort(4444)
                    .Attach()
                    .WithDnsPrefix(randomContainerInstance)
                    .WithRestartPolicy(ContainerGroupRestartPolicy.Never)
                    .Create();
                return true;
            }
            catch (Exception ex)
            {
                context.WriteLine(ex.Message);
                containerGroup = null;
                randomContainerInstance = null;
                return false;
            }
        }

        public void Stop(PerformContext context)
        {
            if (string.IsNullOrWhiteSpace(_createdContainerGroupId))
            {
                context.WriteLine("I was expecting a container; There is no Id, couldn't stop anything");
                return;
            }

            lock (_createdContainerGroupId)
            {

                IAzure azure = GetAzureContext(context);
                DoStop(azure, _createdContainerGroupId, context);

                _createdContainerGroupId = null;
                ContainerSeleniumUrl = null;
            }
        }

        private void DoStop(IAzure azure, string containerGroupId, PerformContext context)
        {
            IContainerGroup containerGroup = azure.ContainerGroups.GetById(containerGroupId);
            context.WriteLine($"Stopping ContainerGroup {containerGroup.Name}");

            containerGroup.Stop();
            context.WriteLine($"Deleting ContainerGroup {containerGroup.Name}");
            azure.ContainerGroups.DeleteById(containerGroup.Id);

        }

        private static IAzure GetAzureContext(PerformContext context)
        {
            IAzure azure;

            try
            {
                context.WriteLine($"Authenticating with Azure using servicePrincipal");

                AzureCredentialsFactory azureCredentialsFactory = new AzureCredentialsFactory();
                string clientId = "29db2293-7e74-4715-9c88-32eece20e270";
                string clientSecret = "6O1koUAfwco+8wZHPmrjeOECSQVgAvyZYiuS+6vPbL0=";
                string tenantId = "b170db8b-8e00-4ad4-a076-ccc84281725d";
                string subscriptionId = "7ebdeab9-8060-4fe5-b6c5-a522c7f206b6";
                AzureCredentials azureCredentials = azureCredentialsFactory.FromServicePrincipal(clientId, clientSecret, tenantId, AzureEnvironment.AzureGlobalCloud);
                azure = Azure.Configure().WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic).WithRetryPolicy(new RetryPolicy(new TransientErrorIgnoreStrategy(), new ExponentialBackoffRetryStrategy())).Authenticate(azureCredentials).WithSubscription(subscriptionId);
                ISubscription sub = azure.GetCurrentSubscription();

                context.WriteLine($"Authenticated with subscription '{sub.DisplayName}' (ID: {sub.SubscriptionId})");
            }
            catch (Exception ex)
            {
                context.SetTextColor(ConsoleTextColor.DarkRed);
                context.WriteLine($"\nFailed to authenticate:\n{ex.Message}");
                context.ResetTextColor();

                throw;
            }

            return azure;
        }

    }
}