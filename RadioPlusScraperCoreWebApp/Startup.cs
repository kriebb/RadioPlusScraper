﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RadioPlusScraperWebApi;
using WebScrapingProject;

namespace RadioPlusScraperCoreWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthenticationCore();
            services.AddHangfire(globalConfig =>
            {
                globalConfig.UseMemoryStorage();


            });
            services.AddMvc();

            services.AddSingleton<IRadioPlusDownloader, RadioPlusDownloader>();
            services.AddSingleton<IRadioPlusDownloadHandler, RadioPlusDownloadHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions() { Authorization = new[] { new MyAuthorizationFilter() } });

            var downloadHandler = app.ApplicationServices.GetService<IRadioPlusDownloadHandler>();
            BackgroundJob.Enqueue(() => downloadHandler.Start());
        }
    }

    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return httpContext.User.Identity.IsAuthenticated;
            return true;
        }
    }
}
