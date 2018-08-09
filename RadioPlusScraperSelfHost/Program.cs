using System;
using System.Diagnostics;
using System.Threading;
using System.Web.Http;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using RadioPlusScraperWebApi;

[assembly: OwinStartup(typeof(RadioPlusScraperSelfHost.HangFireStartup))]

namespace RadioPlusScraperSelfHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string baseAddress = "http://*:9000";


            // Start OWIN host 
            using (WebApp.Start<AppBuilderConfigurator>(url: baseAddress))
            {
                RadioPlusDownloadHandler.Start();

                while (true)
                {
                    Thread.Sleep(10000);
                    Trace.TraceInformation("Working", "Hosting using Owin WebApp");
                }
            }
        }
    }

    public class AppBuilderConfigurator
    {
        public void Configuration(IAppBuilder app)
        {
            new OwinWebApiStartup().Configuration(app);
            new HangFireStartup().Configuration(app);
        }
    }
    internal class HangFireStartup
    {
        public void Configuration(IAppBuilder app)
        {
            JobStorage.Current = new MemoryStorage(new MemoryStorageOptions() { });
            app.UseHangfireDashboard("/hangfire", new DashboardOptions() { Authorization = new IDashboardAuthorizationFilter[0] { } }, JobStorage.Current);
            app.UseHangfireServer(new BackgroundJobServerOptions() { }, JobStorage.Current);
        }
    }

    internal class OwinWebApiStartup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }

}
