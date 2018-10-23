using System;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;
using RadioPlusOnDemand.Json;
using RadioPlusScraperWebApi;
using Selenium.WebDriver.WaitExtensions;

namespace WebScrapingProject
{
    public class RadioPlusWebContentWebContentDownloader : IRadioPlusWebContentDownloader
    {
        private readonly IConfiguration _configuration;

        public RadioPlusWebContentWebContentDownloader(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public RadioPlusOnDemandData[] GetOnDemandMaterialJson(string channelUrl)
        {
            try
            {
                string onDemandMaterialJson;

                //var listOfObjects = new List<string>();

                var remoteAddress = _configuration["remoteWebDriver:remoteAddress"];
                using (var driver = new RemoteWebDriver(new Uri(DockerContainerHandler.ContainerSeleniumUrl), new ChromeOptions() { AcceptInsecureCertificates = true }))
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
                throw new Exception("Something went wrong when try to download the ondemandmaterialjson: " + e.Message, e);
            }

        }
    }
}