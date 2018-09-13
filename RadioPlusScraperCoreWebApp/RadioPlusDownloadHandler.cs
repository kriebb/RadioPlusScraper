using RadioPlusOnDemand.Json;
using RadioPlusScraperWebApi;
using WebScrapingProject;

namespace RadioPlusScraperCoreWebApp
{
    internal class RadioPlusDownloadHandler : IRadioPlusDownloadHandler
    {
        private IRadioPlusDownloadOrchestrator _radioPlusDownloadOrchestrator;

        public RadioPlusDownloadHandler(IRadioPlusDownloadOrchestrator radioPlusDownloadOrchestrator)
        {
            this._radioPlusDownloadOrchestrator = radioPlusDownloadOrchestrator;
        }

        public static RadioPlusOnDemandData[] DownloadResult { get; set; } = new RadioPlusOnDemandData[0];

        public void Start()
        {
            _radioPlusDownloadOrchestrator.Start();
        }
    }
}