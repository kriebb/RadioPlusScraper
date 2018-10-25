using System.Diagnostics;
using Hangfire.Dashboard;

namespace RadioPlusScraperCoreWebApp.HangfireManagement
{
    public class NoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            Trace.WriteLine($"Identity Name: {httpContext.User.Identity.Name}");
            Trace.WriteLine($"Authentication Type: {httpContext.User.Identity.AuthenticationType}");
            Trace.WriteLine($"IsAuthenticated: {httpContext.User.Identity.IsAuthenticated}");

            //TODO: how to authenticate?
            return true;
        }
    }
}