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
    public class RadioPlus
    {
        public const string RadioPlusOnDemandUrl = "http://radioplus.be/#/radio1/herbeluister/";

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


                var remoteAddress = _configuration["remoteWebDriver:remoteAddress"];
                using (var driver = new RemoteWebDriver(new Uri(remoteAddress),new ChromeOptions(){AcceptInsecureCertificates = true}))
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


    public class RssManager
    {

        public string GetRssString(string radio, string channelTitle, Guid channelId, RadioPlusOnDemandData[] onDemandMaterialData)
        {
            string channelUrl = Url.Combine(RadioPlus.RadioPlusOnDemandUrl, channelId.ToString());


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
            feed.AlternateLink = RadioPlus.RadioPlusOnDemandUrl;
            feed.Copyright = "vrt";
            feed.ImageUrl = "https://lh4.ggpht.com/CdCC7h9Ft5sftidozzTWggH9mUwbdBHO_1ZZCuk_O_A_2TpDuxqxqpf9OAE91LpkBw";
            feed.Author = "vrt";
            feed.WebMaster = new FeedEmailAddress("info@radioplus.be", "RadioPlus");
            feed.ImageTitle = title;
            feed.Link = RadioPlus.RadioPlusOnDemandUrl;
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
                    string channelUrl = Url.Combine(RadioPlus.RadioPlusOnDemandUrl, channel.CollectionId.ToString());
                    var feedItem = MapToEpisode(episode, channelUrl); //do more with async
                    feedItems.Add(feedItem);
                }

                feed.Items = feedItems;
            }

            return feed.WriteRSS();
        }
    }




}
