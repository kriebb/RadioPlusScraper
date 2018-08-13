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
        public List<string> JobIds = new List<string>();
        public void Start()
        {
            TimeSpan timeSpanRetry = TimeSpan.Zero;
            Console.WriteLine(nameof(RadioPlusDownloadHandler) + "." + nameof(Start));
            try
            {
                lock (DownloadResultLock)
                {
                    DownloadResult =
                        _radioPlusDownloader.GetOnDemandMaterialJson(WebScrapingProject.RadioPlus.RadioPlusOnDemandUrl);
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

                JobIds.Add(jobId);
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
