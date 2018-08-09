using System;
using System.Collections.Generic;
using Hangfire;
using RadioPlusOnDemand.Json;
using WebScrapingProject;

namespace RadioPlusScraperWebApi
{
    public static class RadioPlusDownloadHandler
    {
        public static List<string> JobIds = new List<string>();
        public static void Start()
        {
            TimeSpan timeSpanRetry = TimeSpan.Zero;
            Console.WriteLine(nameof(RadioPlusDownloadHandler)+"."+nameof(Start));
            try
            {
                lock (DownloadResultLock)
                {
                    var downloader = new RadioPlusDownloader();
                    DownloadResult =
                        downloader.GetOnDemandMaterialJson(WebScrapingProject.RadioPlus.RadioPlusOnDemandUrl);
                    timeSpanRetry = TimeSpan.FromHours(1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                timeSpanRetry = TimeSpan.FromSeconds(1);
            }
            finally
            {
                Console.WriteLine("Retrying next:"+timeSpanRetry.ToString());
            }
            try
            {
                var jobId = BackgroundJob.Schedule(() => RadioPlusDownloadHandler.Start(), timeSpanRetry);
                Console.WriteLine($"Scheduling job with id:{jobId} ");

                JobIds.Add(jobId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        private static Object DownloadResultLock = new Object();
        public static RadioPlusOnDemandData[] DownloadResult { get; set; } = new RadioPlusOnDemandData[0];
    }
}
