using System;

namespace RadioPlusScraperCoreWebApp
{
    internal interface IRadioPlusDownloadHandler
    {
        void Start();
        TimeSpan NextTimeSpan { get; } //should be static, because we don't have a repository that will get a single value. Hangfire makes sure everything is transient
    }
}