using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RadioPlusScraperWebApi;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Hangfire.Console;
using WebScrapingProject;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace RadioPlusScraperCoreWebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();

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
            services.AddSingleton<IConfiguration>(serviceProvider => Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireServer(new BackgroundJobServerOptions() { Queues = new string[] { "Download", "default" }, WorkerCount = 1 });
            app.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = new[] { new MyAuthorizationFilter() } });
            app.UseMvc();

        }
    }

    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            Microsoft.AspNetCore.Http.HttpContext httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return httpContext.User.Identity.IsAuthenticated;
            return true;
        }
    }

    internal class InitRadioPlusOrchestratorHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IRadioPlusDownloadScheduler _scheduler;

        public InitRadioPlusOrchestratorHostedService(ILogger<InitRadioPlusOrchestratorHostedService> logger, IRadioPlusDownloadScheduler scheduler)
        {
            _logger = logger;
            _scheduler = scheduler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            try
            {
                _scheduler.ScheduleJob(TimeSpan.Zero);

            }
            catch (Exception e)
            {
                Trace.Fail("Could not start the Orchestrator", e.Message + System.Environment.NewLine + e.StackTrace);
            }

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
