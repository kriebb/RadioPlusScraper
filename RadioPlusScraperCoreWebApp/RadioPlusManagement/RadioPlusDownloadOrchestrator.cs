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
        private readonly IRadioPlusWebContentDownloader _radioPlusWebContentDownloader;
        private readonly IDockerContainerHandler _dockerContainerHandler;

        public RadioPlusDownloadOrchestrator(IRadioPlusWebContentDownloader radioPlusWebContentDownloader, IDockerContainerHandler dockerContainerHandler)
        {
            _radioPlusWebContentDownloader = radioPlusWebContentDownloader;
            _dockerContainerHandler = dockerContainerHandler;
        }

        public void Start()
        {
            var backGroundClient = new BackgroundJobClient();
            var startDockerJobId = backGroundClient.Create(() => _dockerContainerHandler.Start(), new EnqueuedState());
            var getInfoJobId = backGroundClient.ContinueWith(startDockerJobId, () => DoStart(), new EnqueuedState());
            var stopDockerJobId = backGroundClient.ContinueWith(getInfoJobId, () => _dockerContainerHandler.Stop(), new EnqueuedState());
            backGroundClient.ContinueWith(stopDockerJobId, () => DoScheduleJob(), new EnqueuedState());
        }

        public void DoScheduleJob()
        {
            BackgroundJob.Schedule(() => this.Start(), NextTimeSpan);
        }

        public static TimeSpan NextTimeSpan = TimeSpan.FromHours(12);

        public void DoStart()
        {
            try
            {
                lock (DownloadResultLock)
                {
                    var myList = new List<RadioPlusOnDemandData>();
                    foreach (var radio in RadioPlusConst.AllRadios)
                    {
                        var items = _radioPlusWebContentDownloader.GetOnDemandMaterialJson(
                            WebScrapingProject.RadioPlusConst.GetRadioPlusOnDemandUrl(radio));
                        myList.AddRange(items);
                    }
                    RadioPlusDownloadHandler.DownloadResult = myList.ToArray();
                    NextTimeSpan = TimeSpan.FromHours(12);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                NextTimeSpan = TimeSpan.FromMinutes(2);
            }
        }

        private static readonly Object DownloadResultLock = new Object();
    }
}
