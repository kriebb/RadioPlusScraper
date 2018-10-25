using Hangfire.Server;
using RadioPlusOnDemand.Json;

namespace WebScrapingProject
{
    public interface IRadioPlusWebContentDownloader
    {
        RadioPlusOnDemandData[] GetOnDemandMaterialJson(string onDemandUrl, PerformContext context);
    }
}