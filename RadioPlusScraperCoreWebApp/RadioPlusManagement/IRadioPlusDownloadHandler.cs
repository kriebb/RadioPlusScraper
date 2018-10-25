using System;
using Hangfire.Server;

namespace RadioPlusScraperCoreWebApp
{
    public interface IRadioPlusDownloadHandler
    {
        void Start(PerformContext context = null);
    }
}