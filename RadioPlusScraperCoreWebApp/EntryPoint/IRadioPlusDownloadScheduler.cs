using System;
using Hangfire.Server;

namespace RadioPlusScraperCoreWebApp.EntryPoint
{
    public interface IRadioPlusDownloadScheduler
    {
        void ScheduleJob(TimeSpan timeSpan, PerformContext context = null);
    }
}