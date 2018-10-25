using System;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using RadioPlusScraperCoreWebApp.JobsOrchestrationManagement;

namespace RadioPlusScraperCoreWebApp.EntryPoint.Impl
{
    public class RadioPlusDownloadScheduler : IRadioPlusDownloadScheduler
    {
        public void ScheduleJob(TimeSpan timeSpan, PerformContext context = null)
        {
            context.WriteLine($"Scheduling job at {DateTime.Now.Add(timeSpan)}");
            BackgroundJob.Schedule<IRadioPlusDownloadOrchestrator>(x => x.Start(null), timeSpan);
        }
    }
}