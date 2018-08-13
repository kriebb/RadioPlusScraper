using System;
using System.Collections.Generic;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using RadioPlusOnDemand.Json;
using WebScrapingProject;

namespace RadioPlusScraperWebApi
{
    public interface IRadioPlusDownloadHandler
    {
        void Start();
    }
    public class RadioPlusDownloadHandler : IRadioPlusDownloadHandler
    {
        private readonly IRadioPlusDownloader _radioPlusDownloader;

        public RadioPlusDownloadHandler(IRadioPlusDownloader radioPlusDownloader)
        {
            _radioPlusDownloader = radioPlusDownloader;
        }

        private readonly List<string> _jobIds = new List<string>();
        public void Start()
        {
            TimeSpan timeSpanRetry = TimeSpan.Zero;
            Console.WriteLine(nameof(RadioPlusDownloadHandler) + "." + nameof(Start));
            try
            {
                lock (DownloadResultLock)
                {
                    var myList = new List<RadioPlusOnDemandData>();
                    foreach (var radio in RadioPlusConst.AllRadios)
                    {
                        var items = _radioPlusDownloader.GetOnDemandMaterialJson(WebScrapingProject.RadioPlusConst.GetRadioPlusOnDemandUrl(radio));
                        myList.AddRange(items);
                    }

                    DownloadResult = myList.ToArray();
                    timeSpanRetry = TimeSpan.FromHours(1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                timeSpanRetry = TimeSpan.FromSeconds(1);
                throw e; //let hangfire retry
            }
            finally
            {
                Console.WriteLine("Retrying next:" + timeSpanRetry.ToString());
            }
            try
            {
                var jobId = BackgroundJob.Schedule(() => this.Start(), timeSpanRetry);
                Console.WriteLine($"Scheduling job with id:{jobId} ");

                _jobIds.Add(jobId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        private static readonly Object DownloadResultLock = new Object();
        public static RadioPlusOnDemandData[] DownloadResult { get; set; } = new RadioPlusOnDemandData[0];
    }
}
