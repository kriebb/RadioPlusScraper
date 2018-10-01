using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Hangfire;
using Hangfire.States;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using RadioPlusOnDemand.Json;
using RadioPlusScraperCoreWebApp;
using WebScrapingProject;

namespace RadioPlusScraperWebApi
{
    internal class RadioPlusDownloadOrchestrator : IRadioPlusDownloadOrchestrator
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
            var startDockerJobId = backGroundClient.Create(() => _dockerContainerHandler.Start(), new EnqueuedState());
            var getInfoJobId = backGroundClient.ContinueWith(startDockerJobId, () => _radioPlusDownloadHandler.Start(), new EnqueuedState());
            var stopDockerJobId = backGroundClient.ContinueWith(getInfoJobId, () => _dockerContainerHandler.Stop(), new EnqueuedState());
            backGroundClient.ContinueWith(stopDockerJobId, () => DoScheduleJob(), new EnqueuedState());
        }

        private void DoScheduleJob()
        {
            BackgroundJob.Schedule(() => this.Start(), _radioPlusDownloadHandler.NextTimeSpan);
        }



    }
}
