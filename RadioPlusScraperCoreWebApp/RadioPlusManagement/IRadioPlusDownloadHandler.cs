using Hangfire.Server;

namespace RadioPlusScraperCoreWebApp.RadioPlusManagement
{
    public interface IRadioPlusDownloadHandler
    {
        void Start(PerformContext context = null);
    }
}