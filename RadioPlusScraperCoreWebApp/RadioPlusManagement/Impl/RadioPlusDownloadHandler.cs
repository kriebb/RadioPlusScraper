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
        private static readonly object DownloadResultLock = new object();
        private readonly IRadioPlusWebContentDownloader _radioPlusWebContentDownloader;

        public RadioPlusDownloadHandler(IRadioPlusWebContentDownloader radioPlusWebContentDownloader)
        {
            _radioPlusWebContentDownloader = radioPlusWebContentDownloader;
        }

        public static RadioPlusOnDemandData[] DownloadResult { get; private set; } = new RadioPlusOnDemandData[0];

        public static TimeSpan NextTimeSpan { get; private set; } = TimeSpan.FromHours(12);

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
                    var bar = context.WriteProgressBar("Radios");
                    var myList = new List<RadioPlusOnDemandData>();
                    foreach (var radio in RadioPlusConst.AllRadios.WithProgress(bar))
                        try
                        {
                            context.WriteLine($"Getting GetOnDemandMaterialJson for radio {radio}");
                            var url = RadioPlusConst.GetRadioPlusOnDemandUrl(radio);
                            context.WriteLine($"Asking items at url {url}");
                            var items = _radioPlusWebContentDownloader.GetOnDemandMaterialJson(url, context);
                            myList.AddRange(items);
                        }
                        catch (Exception e)
                        {
                            context.SetTextColor(ConsoleTextColor.DarkRed);
                            context.WriteLine(
                                $"An error occured while trying to download the information for radio {radio}");
                            context.WriteLine(e.Message);
                            context.WriteLine(e.StackTrace);
                            context.ResetTextColor();
                        }

                    DownloadResult = myList.ToArray();
                    NextTimeSpan = TimeSpan.FromHours(12);
                }
            }
            catch (Exception e)
            {
                context.SetTextColor(ConsoleTextColor.DarkRed);
                context.WriteLine("An error occured while trying to download the information");
                context.WriteLine(e.Message);
                context.WriteLine(e.StackTrace);
                context.ResetTextColor();

                NextTimeSpan = TimeSpan.FromMinutes(2);
            }
        }
    }
}