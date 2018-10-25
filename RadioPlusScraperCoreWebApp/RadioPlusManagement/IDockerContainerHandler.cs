using Hangfire.Server;

namespace RadioPlusScraperWebApi
{
    public interface IDockerContainerHandler
    {

        void Start(PerformContext context = null);
        void Stop(PerformContext context = null);
    }
}