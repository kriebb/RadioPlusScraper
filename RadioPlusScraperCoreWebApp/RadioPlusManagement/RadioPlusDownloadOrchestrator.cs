using Hangfire;
using Hangfire.States;
using RadioPlusScraperCoreWebApp;

namespace RadioPlusScraperWebApi
{
    public class RadioPlusDownloadOrchestrator : IRadioPlusDownloadOrchestrator
    {
        private readonly IRadioPlusDownloadHandler _radioPlusDownloadHandler;
        private readonly IDockerContainerHandler _dockerContainerHandler;

        public RadioPlusDownloadOrchestrator(IRadioPlusDownloadHandler radioPlusDownloadHandler, IDockerContainerHandler dockerContainerHandler)
        {
            _radioPlusDownloadHandler = radioPlusDownloadHandler;
            _dockerContainerHandler = dockerContainerHandler;
        }

        public void Start()
        {
            var backGroundClient = new BackgroundJobClient();
            var startDockerJobId = backGroundClient.Create(() => _dockerContainerHandler.Start(), new EnqueuedState(_radioPlusDownloadHandler.NextTimeSpan.ToString()));
            var getInfoJobId = backGroundClient.ContinueWith(startDockerJobId, () => _radioPlusDownloadHandler.Start(), new EnqueuedState(_radioPlusDownloadHandler.NextTimeSpan.ToString()));
            var stopDockerJobId = backGroundClient.ContinueWith(getInfoJobId, () => _dockerContainerHandler.Stop(), new EnqueuedState(_radioPlusDownloadHandler.NextTimeSpan.ToString()));
            backGroundClient.ContinueWith(stopDockerJobId, () => DoScheduleJob(backGroundClient), new EnqueuedState(_radioPlusDownloadHandler.NextTimeSpan.ToString()));
        }

        private void DoScheduleJob(BackgroundJobClient backGroundClient)
        {
            backGroundClient.Schedule(() => Start(), _radioPlusDownloadHandler.NextTimeSpan);
        }



    }
}
