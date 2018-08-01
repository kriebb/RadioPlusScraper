using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.Extensions;
using RadioPlusOnDemand.Json;
using Selenium.WebDriver.WaitExtensions;
using Uncas.Web;


namespace WebScrapingProject
{
    public class RssManager
    {
        private const string RadioPlusOnDemandUrl = "http://radioplus.be/#/radio1/herbeluister/";

        public string GetRssString(string radio, string channelTitle, Guid channelId)
        {
            string channelUrl = Url.Combine(RadioPlusOnDemandUrl, channelId.ToString());

            var onDemandMaterialJson = GetOnDemandMaterialJson(RadioPlusOnDemandUrl);

            var onDemandMaterialData = RadioPlusOnDemandData.FromJson(onDemandMaterialJson);

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

        private static string GetOnDemandMaterialJson(string channelUrl)
        {
            string onDemandMaterialJson;
            using (var driver = new PhantomJSDriver())
            {
                driver.Navigate().GoToUrl(channelUrl);

                driver.Wait().ForPage().ReadyStateComplete();
                driver.Wait(10000)
                    .ForElement(
                        By.CssSelector("#content > div > div.content-view > ul > li > a > span > span.episodes"))
                    .ToExist();

                onDemandMaterialJson =
                    driver.ExecuteJavaScript<string>(
                        "return (JSON.stringify(require('app').currentView.content.channel.ondemand));");
            }

            return onDemandMaterialJson;
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
            var webRequest = WebRequest.Create(uriPath);
            webRequest.Method = "HEAD";

            using (var webResponse = webRequest.GetResponse())
            {
                var fileSizeInBytes = webResponse.Headers.Get("Content-Length");
                if (int.TryParse(fileSizeInBytes, out var fileSizeInBytesInt))
                {
                    return fileSizeInBytesInt;
                }
                return 1;
            }
        }

        public string GetRssString(string radio, string title, string description)
        {
            var onDemandMaterialJson = GetOnDemandMaterialJson(RadioPlusOnDemandUrl);

            var onDemandMaterialData = RadioPlusOnDemandData.FromJson(onDemandMaterialJson);

            var feed = new Feed(title: radio, description: description);
            feed.AlternateLink = RadioPlusOnDemandUrl;
            feed.Copyright = "vrt";
            feed.ImageUrl = "https://lh4.ggpht.com/CdCC7h9Ft5sftidozzTWggH9mUwbdBHO_1ZZCuk_O_A_2TpDuxqxqpf9OAE91LpkBw";
            feed.Author = "vrt";
            feed.WebMaster = new FeedEmailAddress("info@radioplus.be", "RadioPlus");
            feed.ImageTitle = title;
            feed.Link = RadioPlusOnDemandUrl;
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
                    string channelUrl = Url.Combine(RadioPlusOnDemandUrl, channel.CollectionId.ToString());
                    var feedItem = MapToEpisode(episode, channelUrl); //do more with async
                    feedItems.Add(feedItem);
                }

                feed.Items = feedItems;
            }

            return feed.WriteRSS();
        }
    }




}
