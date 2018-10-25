using Hangfire.Server;

namespace RadioPlusScraperCoreWebApp.DockerManagement
{
    public interface IDockerContainerHandler
    {
        void Start(PerformContext context = null);
        void Stop(PerformContext context = null);
    }
}