using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using RadioPlusScraperCoreWebApp.DockerManagement;
using RadioPlusScraperCoreWebApp.EntryPoint;
using RadioPlusScraperCoreWebApp.RadioPlusManagement;
using RadioPlusScraperCoreWebApp.RadioPlusManagement.Impl;

namespace RadioPlusScraperCoreWebApp.JobsOrchestrationManagement
{
    public class RadioPlusDownloadOrchestrator : IRadioPlusDownloadOrchestrator
    {
        [DisableConcurrentExecution(300)]
        public void Start(PerformContext context = null)
        {
            context.WriteLine(
                "Setting up the jobs with continuation in order to start the container, download info, stop the container and reschedule the job");
            var startDockerJobId = BackgroundJob.Enqueue<IDockerContainerHandler>(x => x.Start(null));
            var getInfoJobId = BackgroundJob.ContinueWith<IRadioPlusDownloadHandler>(startDockerJobId,
                x => x.Start(null), JobContinuationOptions.OnlyOnSucceededState); //,downloadQueue);
            var stopDockerJobId = BackgroundJob.ContinueWith<IDockerContainerHandler>(getInfoJobId, x => x.Stop(null),
                JobContinuationOptions.OnAnyFinishedState);
            BackgroundJob.ContinueWith<IRadioPlusDownloadScheduler>(stopDockerJobId,
                x => x.ScheduleJob(RadioPlusDownloadHandler.NextTimeSpan, null),
                JobContinuationOptions.OnAnyFinishedState);
        }
    }
}