using System;
using System.Collections.Generic;
using RadioPlusOnDemand.Json;
using WebScrapingProject;

namespace RadioPlusScraperCoreWebApp
{
    public class RadioPlusDownloadHandler : IRadioPlusDownloadHandler
    {
        private readonly IRadioPlusWebContentDownloader _radioPlusWebContentDownloader;

        public RadioPlusDownloadHandler(IRadioPlusWebContentDownloader radioPlusWebContentDownloader)
        {
            _radioPlusWebContentDownloader = radioPlusWebContentDownloader;
        }

        public static RadioPlusOnDemandData[] DownloadResult { get; private set; } = new RadioPlusOnDemandData[0];

        public void Start()
        {
            DoStart();
        }


        private void DoStart()
        {
            try
            {
                lock (DownloadResultLock)
                {
                    var myList = new List<RadioPlusOnDemandData>();
                    foreach (var radio in RadioPlusConst.AllRadios)
                    {
                        var items = _radioPlusWebContentDownloader.GetOnDemandMaterialJson(
                            RadioPlusConst.GetRadioPlusOnDemandUrl(radio));
                        myList.AddRange(items);
                    }
                    DownloadResult = myList.ToArray();
                    NextTimeSpan = TimeSpan.FromHours(12);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                NextTimeSpan = TimeSpan.FromMinutes(2);
            }
        }

        private static TimeSpan _nextTimeSpan = TimeSpan.FromHours(12);
        public TimeSpan NextTimeSpan
        {
            get => _nextTimeSpan;
            private set => _nextTimeSpan = value;
        } 


        private static readonly Object DownloadResultLock = new Object();
    }
}