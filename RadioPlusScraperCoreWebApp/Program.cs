﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using RadioPlusScraperCoreWebApp.EntryPoint;

namespace RadioPlusScraperCoreWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}