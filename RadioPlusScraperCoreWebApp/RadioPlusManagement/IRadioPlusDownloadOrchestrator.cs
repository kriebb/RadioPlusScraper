using Hangfire.Server;

namespace RadioPlusScraperWebApi
{
    public interface IRadioPlusDownloadOrchestrator
    {
        void Start(PerformContext context = null);
    }
}