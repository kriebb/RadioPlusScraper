using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using RadioPlusScraperCoreWebApp;
using System;

namespace RadioPlusScraperWebApi
{
    public class RadioPlusDownloadScheduler : IRadioPlusDownloadScheduler
    {
        public void ScheduleJob(TimeSpan timeSpan, PerformContext context = null)
        {
            BackgroundJob.Schedule<IRadioPlusDownloadOrchestrator>(x => x.Start(null), timeSpan);
        }
    }

    public interface IRadioPlusDownloadScheduler
    {
        void ScheduleJob(TimeSpan timeSpan, PerformContext context = null);
    }
    public class RadioPlusDownloadOrchestrator : IRadioPlusDownloadOrchestrator
    {
        [Hangfire.DisableConcurrentExecution(300)]
        public void Start(PerformContext context = null)
        {
            context.WriteLine("Setting up the jobs with continuation in order to start the container, download info, stop the container and reschedule the job");
            string startDockerJobId = BackgroundJob.Enqueue<IDockerContainerHandler>(x => x.Start(null));
            string getInfoJobId = BackgroundJob.ContinueWith<IRadioPlusDownloadHandler>(startDockerJobId, x => x.Start(null), JobContinuationOptions.OnlyOnSucceededState);//,downloadQueue);
            string stopDockerJobId = BackgroundJob.ContinueWith<IDockerContainerHandler>(getInfoJobId, x => x.Stop(null), JobContinuationOptions.OnAnyFinishedState);
            BackgroundJob.ContinueWith<IRadioPlusDownloadScheduler>(stopDockerJobId, x => x.ScheduleJob(RadioPlusDownloadHandler.NextTimeSpan, null), JobContinuationOptions.OnAnyFinishedState);
        }
    }
}
