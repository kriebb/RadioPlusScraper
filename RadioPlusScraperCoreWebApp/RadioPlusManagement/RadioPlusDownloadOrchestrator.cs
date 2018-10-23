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
            var startDockerJobId = backGroundClient.Create(() => _dockerContainerHandler.Start(), new EnqueuedState(_radioPlusDownloadHandler.NextTimeSpan.ToString().Replace(":", "_")));
            var getInfoJobId = backGroundClient.ContinueWith(startDockerJobId, () => _radioPlusDownloadHandler.Start(), new EnqueuedState(_radioPlusDownloadHandler.NextTimeSpan.ToString().Replace(":", "_")));
            var stopDockerJobId = backGroundClient.ContinueWith(getInfoJobId, () => _dockerContainerHandler.Stop(), new EnqueuedState(_radioPlusDownloadHandler.NextTimeSpan.ToString().Replace(":", "_")));
            backGroundClient.ContinueWith(stopDockerJobId, () => DoScheduleJob(), new EnqueuedState(_radioPlusDownloadHandler.NextTimeSpan.ToString().Replace(":", "_")));
        }

        public void DoScheduleJob()
        {
            var backGroundClient = new BackgroundJobClient();
            backGroundClient.Schedule(() => Start(), _radioPlusDownloadHandler.NextTimeSpan);
        }



    }
}
