using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using Hangfire;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using RadioPlusOnDemand.Json;
using Selenium.WebDriver.WaitExtensions;
using Uncas.Web;


namespace WebScrapingProject
{
    public class RadioPlusConst
    {
        private const string RadioPlusOnDemandUrl = "http://radioplus.be/#/{0}/herbeluister/";

        public static string GetRadioPlusOnDemandUrl(string radio)
        {
            string ondemandUrl = string.Format(RadioPlusOnDemandUrl, radio);
            return ondemandUrl;
        }

        public static string Radio1 = "radio1";
        public static string Radio2 = "radio2-vlaams-brabant";
        public static string[] AllRadios = { Radio1, Radio2 };

    }

    public interface IRadioPlusDownloader
    {
        RadioPlusOnDemandData[] GetOnDemandMaterialJson(string onDemandUrl);
    }
    public class RadioPlusDownloader : IRadioPlusDownloader
    {
        private readonly IConfiguration _configuration;

        public RadioPlusDownloader(IConfiguration configuration)
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
                using (var driver = new RemoteWebDriver(new Uri(remoteAddress), new ChromeOptions() { AcceptInsecureCertificates = true }))
                {
                    driver.Navigate().GoToUrl(channelUrl);
                    driver.Wait(6000000).ForPage().ReadyStateComplete();
                    driver.Wait(6000000)
                        .ForElement(
                            By.CssSelector("#content > div > div.content-view > ul > li > a > span > span.episodes"))
                        .ToExist();
                    /*
                    var length = driver.ExecuteJavaScript<string>("return \"\"+ (require('app').currentView.content.channel.ondemand.length);");

                    var getSlice =
                        "return (JSON.stringify(require('app').currentView.content.channel.ondemand.slice({0},{1}));";
                    if (int.TryParse(length, out var intLength))
                    {
                        int min = 0;
                        int max = 10;
                        do
                        {
                            min = max;
                            max += 10;
                            if (max > intLength)
                                max = intLength - 1;

                            var slicing = string.Format(getSlice, min, max);
                            var onDemandMaterialJsonPart = driver.ExecuteJavaScript<string>(slicing);
                            listOfObjects.Add(onDemandMaterialJsonPart);
                        } while (max < intLength);



                    }
                    */

                    onDemandMaterialJson =
                        driver.ExecuteJavaScript<string>(
                            "return (JSON.stringify(require('app').currentView.content.channel.ondemand));");

                }

                var onDemandMaterialData = RadioPlusOnDemandData.FromJson(onDemandMaterialJson);
                /*
                var listOfMaterial = new List<RadioPlusOnDemandData[]>();
                var tempArray = listOfObjects.Select(RadioPlusOnDemandData.FromJson);
                listOfMaterial.AddRange(tempArray);
                var onDemandMaterialData = listOfMaterial.SelectMany(x => x).ToArray();
                */

                return onDemandMaterialData;
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong when try to download the ondemandmaterialjson: " + e.Message, e);
            }

        }
    }


    public class RssManager
    {

        public string GetRssString(string radio, string channelTitle, Guid channelId, RadioPlusOnDemandData[] onDemandMaterialData)
        {
            string ondemandUrl = RadioPlusConst.GetRadioPlusOnDemandUrl(radio);
            string channelUrl = Url.Combine(ondemandUrl, channelId.ToString());


            var channel = onDemandMaterialData.FirstOrDefault(x => x.CollectionId == channelId);

            var feed = new Feed(title: radio, description: channelTitle);
            feed.AlternateLink = channelUrl;
            feed.Copyright = "vrt";
            feed.ImageUrl = channel?.Thumb;
            feed.Author = "vrt";
            feed.WebMaster = new FeedEmailAddress("info@radioplus.be", "RadioPlus");
            feed.ImageTitle = channelTitle;
            feed.Link = channelUrl;
            feed.Language = "nl-BE";

            if (channel == null)
                return feed.WriteRSS();

            var feedItems = new List<FeedItem>();

            foreach (var episode in channel.Items)
            {
                var feedItem = MapToEpisode(episode, channelUrl); //do more with async
                feedItems.Add(feedItem);
            }

            feed.Items = feedItems;

            return feed.WriteRSS();

        }

        private static FeedItem MapToEpisode(Item episode, string channelUrl)
        {
            var rssEpisode = new FeedItem(episode.Title, Url.Combine(channelUrl, episode.Id.ToString()));

            var length = GetFileSizeAsync(new Uri(episode.Stream));

            rssEpisode.Enclosure = new FeedItemEnclosure(
                url: episode.Stream,
                mimeType: "audio/" + Path.GetExtension(episode.Stream)?.Replace(".", ""),
                length: length);

            rssEpisode.Author = new FeedEmailAddress("info@radioplus.be", "RadioPlus");
            rssEpisode.Description = episode.Description;
            rssEpisode.PublishDate = episode.CreationTimestamp.UtcDateTime;
            rssEpisode.Category = episode.Title;

            return rssEpisode;


        }

        private static int GetFileSizeAsync(Uri uriPath)
        {
            try
            {
                var webRequest = WebRequest.Create(uriPath);
                webRequest.Method = "HEAD";

                using (var webResponse = webRequest.GetResponse())
                {
                    var fileSizeInBytes = webResponse.Headers.Get("Content-Length");
                    if (int.TryParse(fileSizeInBytes, out var fileSizeInBytesInt))
                    {
                        return fileSizeInBytesInt;
                    }
                    return 10;
                }
            }
            catch (Exception e)
            {
                return 20;
            }

        }

        public string GetRssString(string radio, string title, string description, RadioPlusOnDemandData[] onDemandMaterialData)
        {
            var feed = new Feed(title: radio, description: description);
            feed.AlternateLink = RadioPlusConst.GetRadioPlusOnDemandUrl(radio);
            feed.Copyright = "vrt";
            feed.ImageUrl = "https://lh4.ggpht.com/CdCC7h9Ft5sftidozzTWggH9mUwbdBHO_1ZZCuk_O_A_2TpDuxqxqpf9OAE91LpkBw";
            feed.Author = "vrt";
            feed.WebMaster = new FeedEmailAddress("info@radioplus.be", "RadioPlus");
            feed.ImageTitle = title;
            feed.Link = RadioPlusConst.GetRadioPlusOnDemandUrl(radio);
            feed.Language = "nl-BE";
            feed.Title = title;
            var feedItems = new List<FeedItem>();

            foreach (var channel in onDemandMaterialData)
            {
                if (channel == null)
                {
                    continue;
                }

                foreach (var episode in channel.Items)
                {
                    string channelUrl = Url.Combine(RadioPlusConst.GetRadioPlusOnDemandUrl(radio), channel.CollectionId.ToString());
                    var feedItem = MapToEpisode(episode, channelUrl); //do more with async
                    feedItems.Add(feedItem);
                }

                feed.Items = feedItems;
            }

            return feed.WriteRSS();
        }
    }




}
