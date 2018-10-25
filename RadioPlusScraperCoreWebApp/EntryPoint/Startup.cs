using Hangfire;
using Hangfire.Console;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RadioPlusScraperCoreWebApp.DockerManagement;
using RadioPlusScraperCoreWebApp.EntryPoint.Impl;
using RadioPlusScraperCoreWebApp.HangfireManagement;
using RadioPlusScraperCoreWebApp.JobsOrchestrationManagement;
using RadioPlusScraperCoreWebApp.RadioPlusManagement;
using RadioPlusScraperCoreWebApp.RadioPlusManagement.Impl;
using WebScrapingProject;

namespace RadioPlusScraperCoreWebApp.EntryPoint
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthenticationCore();
            services.AddHangfire(globalConfig =>
            {
                globalConfig.UseMemoryStorage();
                globalConfig.UseConsole();
            });
            services.AddMvc().AddControllersAsServices().WithRazorPagesAtContentRoot();

            services.AddLogging(builder =>
                builder.AddDebug().AddConsole().AddAzureWebAppDiagnostics());
            services.AddHostedService<InitRadioPlusOrchestratorHostedService>();

            services.AddSingleton<IRadioPlusWebContentDownloader, RadioPlusWebContentWebContentDownloader>();
            services.AddSingleton<IRadioPlusDownloadHandler, RadioPlusDownloadHandler>();
            services.AddSingleton<IRadioPlusDownloadOrchestrator, RadioPlusDownloadOrchestrator>();
            services.AddSingleton<IRadioPlusDownloadScheduler, RadioPlusDownloadScheduler>();

            services.AddSingleton<IDockerContainerHandler, DockerContainerHandler>();
            services.AddSingleton(serviceProvider => Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHangfireServer(new BackgroundJobServerOptions
                {Queues = new[] {"Download", "default"}, WorkerCount = 1});
            app.UseHangfireDashboard("/hangfire",
                new DashboardOptions {Authorization = new[] {new NoAuthorizationFilter()}});
            app.UseMvc();
        }
    }
}