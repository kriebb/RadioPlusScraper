using RadioPlusOnDemand.Json;

namespace WebScrapingProject
{
    public interface IRadioPlusWebContentDownloader
    {
        RadioPlusOnDemandData[] GetOnDemandMaterialJson(string onDemandUrl);
    }
}