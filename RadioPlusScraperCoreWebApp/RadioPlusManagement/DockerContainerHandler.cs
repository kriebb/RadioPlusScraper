using System;
using Microsoft.Azure.Management.ContainerInstance.Fluent.Models;
using Microsoft.Azure.Management.ContainerRegistry.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Rest.TransientFaultHandling;

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
        public void Start()
        {
            if (!string.IsNullOrWhiteSpace(_createdContainerGroupId)) return;
            _createdContainerGroupId = "not empty";

            lock (_createdContainerGroupId)
            {

                var azure = GetAzureContext();


                RunTaskBasedContainer(azure);
            }
        }

        private void RunTaskBasedContainer(IAzure azure)
        {
            Console.WriteLine($"\nCreating container group '{containerGroupName}'");

            Region azureRegion = Region.EuropeWest;

            IRegistry azureRegistry = azure.ContainerRegistries.GetByResourceGroup(resourceGroupName, containerRegistryName);

            string privateRepoUrl = azureRegistry.LoginServerUrl + "/" + seleniumRepositoryName;

            var acrCredentials = azureRegistry.GetCredentials();
            // Create the container group

            var randomContainerGroupName = SdkContext.RandomResourceName(containerGroupName, 63);
            var randomContainerInstance = SdkContext.RandomResourceName(seleniumRepositoryName + "-instance-", 63);
            var containerGroup = azure.ContainerGroups.Define(randomContainerGroupName)
                .WithRegion(azureRegion)
                .WithExistingResourceGroup(resourceGroupName)
                .WithLinux()
                .WithPrivateImageRegistry(azureRegistry.LoginServerUrl, acrCredentials.Username, acrCredentials.AccessKeys[AccessKeyType.Primary])
                .WithoutVolume()
                .DefineContainerInstance(randomContainerInstance)
                .WithImage(privateRepoUrl)
                .WithExternalTcpPort(4444)
                .Attach()
                .WithDnsPrefix(randomContainerInstance)
                .WithRestartPolicy(ContainerGroupRestartPolicy.Never)
                .Create();

            // Print the container's logs
            Console.WriteLine($"Logs for container '{randomContainerInstance}':");
            Console.WriteLine(containerGroup.GetLogContent(randomContainerInstance));

            _createdContainerGroupId = containerGroup.Id;
            ContainerSeleniumUrl = containerGroup.Fqdn;
        }

        public void Stop()
        {
            if (string.IsNullOrWhiteSpace(_createdContainerGroupId)) return;
            lock (_createdContainerGroupId)
            {
                var azure = GetAzureContext();

                var containerGroup = azure.ContainerGroups.GetById(_createdContainerGroupId);
                Console.WriteLine($"Stopping ContainerGroup {containerGroup.Name}");

                containerGroup.Stop();
                Console.WriteLine($"Deleting ContainerGroup {containerGroup.Name}");
                azure.ContainerGroups.DeleteById(containerGroup.Id);
                _createdContainerGroupId = null;
                ContainerSeleniumUrl = null;
            }
        }

        private static IAzure GetAzureContext()
        {
            IAzure azure;

            try
            {
                Console.WriteLine($"Authenticating with Azure using servicePrincipal");

                var azureCredentialsFactory = new AzureCredentialsFactory();
                var clientId = "29db2293-7e74-4715-9c88-32eece20e270";
                var clientSecret = "6O1koUAfwco+8wZHPmrjeOECSQVgAvyZYiuS+6vPbL0=";
                var tenantId = "b170db8b-8e00-4ad4-a076-ccc84281725d";
                var subscriptionId = "7ebdeab9-8060-4fe5-b6c5-a522c7f206b6";
                var azureCredentials = azureCredentialsFactory.FromServicePrincipal(clientId, clientSecret, tenantId, AzureEnvironment.AzureGlobalCloud);
                azure = Azure.Configure().WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic).WithRetryPolicy(new RetryPolicy(new TransientErrorIgnoreStrategy(), new ExponentialBackoffRetryStrategy())).Authenticate(azureCredentials).WithSubscription(subscriptionId);
                var sub = azure.GetCurrentSubscription();

                Console.WriteLine($"Authenticated with subscription '{sub.DisplayName}' (ID: {sub.SubscriptionId})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nFailed to authenticate:\n{ex.Message}");


                throw;
            }

            return azure;
        }

    }
}