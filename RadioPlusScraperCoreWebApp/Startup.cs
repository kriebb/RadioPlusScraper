using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RadioPlusScraperWebApi;
using WebScrapingProject;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace RadioPlusScraperCoreWebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

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


            });
            services.AddMvc().AddControllersAsServices();
            services.AddLogging(builder =>
                builder.AddDebug().AddConsole().AddAzureWebAppDiagnostics());
            services.AddHostedService<InitRadioPlusOrchestratorHostedService>();

            services.AddSingleton<IRadioPlusWebContentDownloader, RadioPlusWebContentWebContentDownloader>();
            services.AddSingleton<IRadioPlusDownloadHandler, RadioPlusDownloadHandler>();
            services.AddSingleton<IRadioPlusDownloadOrchestrator, RadioPlusDownloadOrchestrator>();
            services.AddSingleton<IDockerContainerHandler, DockerContainerHandler>();
            services.AddSingleton<IConfiguration>(serviceProvider => Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireServer(new BackgroundJobServerOptions() { Queues = new string[] { "Download" } });
            app.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = new[] { new MyAuthorizationFilter() } });


        }
    }

    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return httpContext.User.Identity.IsAuthenticated;
            return true;
        }
    }

    internal class InitRadioPlusOrchestratorHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IRadioPlusDownloadOrchestrator _orchestrator;

        public InitRadioPlusOrchestratorHostedService(ILogger<InitRadioPlusOrchestratorHostedService> logger, IRadioPlusDownloadOrchestrator orchestrator)
        {
            _logger = logger;
            _orchestrator = orchestrator;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            BackgroundJob.Schedule(() => _orchestrator.Start(), TimeSpan.Zero);

            return Task.CompletedTask;
        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");


            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
