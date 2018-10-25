using System;
using Hangfire.Console;
using Hangfire.Server;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;
using RadioPlusOnDemand.Json;
using RadioPlusScraperCoreWebApp.DockerManagement;
using Selenium.WebDriver.WaitExtensions;
using WebScrapingProject;

namespace RadioPlusScraperCoreWebApp.RadioPlusManagement.Impl
{
    public class RadioPlusWebContentWebContentDownloader : IRadioPlusWebContentDownloader
    {
        public RadioPlusOnDemandData[] GetOnDemandMaterialJson(string channelUrl, PerformContext context)
        {
            try
            {
                string onDemandMaterialJson;

                context.WriteLine($"using ContainerSeleniumUrl: {DockerContainerHandler.ContainerSeleniumUrl}");

                using (var driver = new RemoteWebDriver(new Uri(DockerContainerHandler.ContainerSeleniumUrl),
                    new ChromeOptions {AcceptInsecureCertificates = true}))
                {
                    driver.Navigate().GoToUrl(channelUrl);
                    driver.Wait(6000000).ForPage().ReadyStateComplete();
                    driver.Wait(6000000)
                        .ForElement(
                            By.CssSelector("#content > div > div.content-view > ul > li > a > span > span.episodes"))
                        .ToExist();


                    onDemandMaterialJson =
                        driver.ExecuteJavaScript<string>(
                            "return (JSON.stringify(require('app').currentView.content.channel.ondemand));");
                }

                var onDemandMaterialData = RadioPlusOnDemandData.FromJson(onDemandMaterialJson);


                return onDemandMaterialData;
            }
            catch (Exception e)
            {
                context.SetTextColor(ConsoleTextColor.DarkRed);
                context.WriteLine(e.Message);
                context.WriteLine(e.StackTrace);
                context.ResetTextColor();

                throw new Exception("Something went wrong when try to download the ondemandmaterialjson: " + e.Message,
                    e);
            }
        }
    }
}