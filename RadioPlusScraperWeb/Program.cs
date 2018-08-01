using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions.Internal;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using RadioPlus.Json;
using RadioPlusOnDemand.Json;
using Selenium.WebDriver.WaitExtensions;
using Uncas.Web;
using Formatting = Newtonsoft.Json.Formatting;


namespace WebScrapingProject
{
    class MainClass
    {
        private const string TitleWeetIkVeel = "#WeetIkVeel";
        private const string ChannelWeetIkVeel = "4f2d4613-41d7-11e6-aa7a-00163edf843f";
        private const string RadioPlusOnDemandUrl = "http://radioplus.be/#/radio1/herbeluister/";
        private const string Radio1 = "radio1";
        public static void Main(string[] args)
        {

            using (var driver = new PhantomJSDriver())
            {
                string channelUrl = Url.Combine(RadioPlusOnDemandUrl, ChannelWeetIkVeel);


                driver.Navigate().GoToUrl(channelUrl);

                driver.Wait().ForPage().ReadyStateComplete();
                driver.Wait(10000).ForElement(By.CssSelector("#content > div > div.content-view > ul > li > a > span > span.episodes")).ToExist();

                driver.GetScreenshot().SaveAsFile(@"d:\appData.png", ImageFormat.Png);


                string onDemandMaterialJson = driver.ExecuteJavaScript<string>("return (JSON.stringify(require('app').currentView.content.channel.ondemand));");

                var onDemandMaterialData = RadioPlusOnDemandData.FromJson(onDemandMaterialJson);

                var channels = onDemandMaterialData.Where(x => x.CollectionId == new Guid(ChannelWeetIkVeel))
                    .ToArray();




                foreach (var channel in channels)
                {
                    var listEpisodes = new List<FeedItem>();

                    var rssGenerator = new Feed(Radio1, TitleWeetIkVeel);
                    rssGenerator.AlternateLink = channelUrl;
                    rssGenerator.Copyright = "vrt";
                    rssGenerator.ImageUrl = channel.Thumb;
                    rssGenerator.Author = "vrt";
                    rssGenerator.WebMaster = new FeedEmailAddress("info@radioplus.be", "RadioPlus");
                    rssGenerator.ImageTitle = TitleWeetIkVeel;
                    rssGenerator.Link = channelUrl;

                    foreach (var episode in channel.Items)
                    {
                        var rssEpisode = MapToEpisode(episode, channelUrl).GetAwaiter().GetResult(); //do more with async
                        listEpisodes.Add(rssEpisode);
                    }

                    rssGenerator.Items = listEpisodes;
                    rssGenerator.Description = TitleWeetIkVeel;


                    string fileName = channel.Title;
                    foreach (var invalidChar in Path.GetInvalidFileNameChars())
                    {
                        fileName = fileName.Replace(invalidChar, '_');

                    }

                    rssGenerator.WriteRSS(@"d:\rssGeneration" + fileName + ".rss.xml");
                    rssGenerator.WriteAtom(@"d:\rssGeneration" + fileName + ".atom.xml");


                }


            }
        }

        private static async Task<FeedItem> MapToEpisode(Item episode, string channelUrl)
        {
            var rssEpisode = new FeedItem(episode.Title, Url.Combine(channelUrl, episode.Id.ToString()));
            rssEpisode.Enclosure = new FeedItemEnclosure(url: episode.Stream, mimeType: "audio/" + Path.GetExtension(episode.Stream)?.Replace(".", ""), length: await GetFileSizeAsync(new Uri(episode.Stream)));
            rssEpisode.Author = new FeedEmailAddress("info@radioplus.be", "RadioPlus");
            rssEpisode.Description = episode.Description;
            rssEpisode.PublishDate = episode.CreationTimestamp.UtcDateTime;
            rssEpisode.Category = string.Concat(Radio1, ";", episode.Title);

            return rssEpisode;
        }

        private static async Task<int> GetFileSizeAsync(Uri uriPath)
        {
            var webRequest = HttpWebRequest.Create(uriPath);
            webRequest.Method = "HEAD";

            using (var webResponse = await webRequest.GetResponseAsync())
            {
                var fileSizeInBytes = webResponse.Headers.Get("Content-Length");
                if (int.TryParse(fileSizeInBytes, out var fileSizeInBytesInt))
                {
                    return fileSizeInBytesInt;
                }
                Debugger.Break();


                return 1;
            }
        }

    }




}
