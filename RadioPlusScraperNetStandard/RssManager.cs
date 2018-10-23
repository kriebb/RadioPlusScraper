using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using Hangfire;
using OpenQA.Selenium.Support.UI;
using RadioPlusOnDemand.Json;
using Uncas.Web;


namespace WebScrapingProject
{
    public class RssManager
    {

        public string GetRssString(string radio, string channelTitle, Guid channelId, RadioPlusOnDemandData[] onDemandMaterialData)
        {
            string ondemandUrl = RadioPlusConst.GetRadioPlusOnDemandUrl(radio);
            string channelUrl = Url.Combine(ondemandUrl, channelId.ToString());


            var channel = onDemandMaterialData.FirstOrDefault(x => x.CollectionId == channelId);

            var feed = new Feed(title: channelTitle + $"({radio})", description: radio)
            {
                AlternateLink = channelUrl,
                Copyright = "vrt",
                ImageUrl = channel?.Thumb,
                Author = "vrt",
                WebMaster = new FeedEmailAddress("info@radioplus.be", "RadioPlus"),
                ImageTitle = channelTitle + $"({radio})",
                Link = channelUrl,
                Language = "nl-BE"
            };

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
            var rssEpisode = new FeedItem(episode.Title + " - " + episode.CreationTimestamp.ToString("dd/MM/yy"), Url.Combine(channelUrl, episode.Id.ToString()));

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
            catch (Exception)
            {
                return 20;
            }

        }

        public string GetRssString(string radio, string title, string description, RadioPlusOnDemandData[] onDemandMaterialData)
        {
            var feed = new Feed(title: radio, description: description)
            {
                AlternateLink = RadioPlusConst.GetRadioPlusOnDemandUrl(radio),
                Copyright = "vrt",
                ImageUrl =
                    "https://lh4.ggpht.com/CdCC7h9Ft5sftidozzTWggH9mUwbdBHO_1ZZCuk_O_A_2TpDuxqxqpf9OAE91LpkBw",
                Author = "vrt",
                WebMaster = new FeedEmailAddress("info@radioplus.be", "RadioPlus"),
                ImageTitle = title,
                Link = RadioPlusConst.GetRadioPlusOnDemandUrl(radio),
                Language = "nl-BE",
                Title = title
            };
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
