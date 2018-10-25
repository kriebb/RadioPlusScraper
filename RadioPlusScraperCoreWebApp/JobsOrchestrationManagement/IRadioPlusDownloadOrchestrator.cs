using Hangfire.Server;

namespace RadioPlusScraperCoreWebApp.JobsOrchestrationManagement
{
    public interface IRadioPlusDownloadOrchestrator
    {
        void Start(PerformContext context = null);
    }
}