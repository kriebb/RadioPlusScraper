using System;
using System.Collections.Generic;
using Hangfire.Console;
using Hangfire.Server;
using RadioPlusOnDemand.Json;
using WebScrapingProject;

namespace RadioPlusScraperCoreWebApp.RadioPlusManagement.Impl
{
    public class RadioPlusDownloadHandler : IRadioPlusDownloadHandler
    {
        private readonly IRadioPlusWebContentDownloader _radioPlusWebContentDownloader;

        public RadioPlusDownloadHandler(IRadioPlusWebContentDownloader radioPlusWebContentDownloader)
        {
            _radioPlusWebContentDownloader = radioPlusWebContentDownloader;
        }

        public static RadioPlusOnDemandData[] DownloadResult { get; private set; } = new RadioPlusOnDemandData[0];

        public void Start(PerformContext context = null)
        {
            DoStart(context);
        }


        private void DoStart(PerformContext context)
        {
            try
            {
                lock (DownloadResultLock)
                {
                    Hangfire.Console.Progress.IProgressBar bar = context.WriteProgressBar("Radios");
                    List<RadioPlusOnDemandData> myList = new List<RadioPlusOnDemandData>();
                    foreach (string radio in RadioPlusConst.AllRadios.WithProgress(bar))
                    {
                        try
                        {
                            context.WriteLine($"Getting GetOnDemandMaterialJson for radio {radio}");
                            string url = RadioPlusConst.GetRadioPlusOnDemandUrl(radio);
                            context.WriteLine($"Asking items at url {url}");
                            RadioPlusOnDemandData[] items = _radioPlusWebContentDownloader.GetOnDemandMaterialJson(url,context);
                            myList.AddRange(items);
                        }
                        catch (Exception e)
                        {
                            context.SetTextColor(ConsoleTextColor.DarkRed);
                            context.WriteLine($"An error occured while trying to download the information for radio {radio}");
                            context.WriteLine(e.Message);
                            context.WriteLine(e.StackTrace);
                            context.ResetTextColor();
                        }
                    }
                    DownloadResult = myList.ToArray();
                    NextTimeSpan = TimeSpan.FromHours(12);

                }
            }
            catch (Exception e)
            {
                context.SetTextColor(ConsoleTextColor.DarkRed);
                context.WriteLine($"An error occured while trying to download the information");
                context.WriteLine(e.Message);
                context.WriteLine(e.StackTrace);
                context.ResetTextColor();

                NextTimeSpan = TimeSpan.FromMinutes(2);
            }
        }

        public static TimeSpan NextTimeSpan { get; private set; } = TimeSpan.FromHours(12);


        private static readonly object DownloadResultLock = new object();
    }
}