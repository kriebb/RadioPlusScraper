using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RadioPlusScraperCoreWebApp.EntryPoint
{
    internal class InitRadioPlusOrchestratorHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IRadioPlusDownloadScheduler _scheduler;

        public InitRadioPlusOrchestratorHostedService(ILogger<InitRadioPlusOrchestratorHostedService> logger,
            IRadioPlusDownloadScheduler scheduler)
        {
            _logger = logger;
            _scheduler = scheduler;
        }

        public void Dispose()
        {
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
                Trace.Fail("Could not start the Orchestrator", e.Message + Environment.NewLine + e.StackTrace);
            }

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");


            return Task.CompletedTask;
        }
    }
}