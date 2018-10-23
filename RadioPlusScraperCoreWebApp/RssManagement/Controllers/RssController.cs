using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RadioPlusScraperCoreWebApp;
using WebScrapingProject;

namespace RadioPlusScraperWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BaseApiController : Controller
    { }

    public class RssController : BaseApiController
    {
        private readonly RssControllerItem[] _rssItemsDoDownload;

        public RssController()
        {
            var rssItemsDoDownload = new List<RssControllerItem>
            {
                new RssControllerItem
                {
                    ChannelId = new Guid("4f2d4613-41d7-11e6-aa7a-00163edf843f"),
                    NameId = "weetikveel",
                    Title = "#WeetIkVeel",
                    RadioStation = RadioPlusConst.Radio1
                },
                new RssControllerItem
                {
                    ChannelId = new Guid("e23e6a65-90d2-11e3-b45a-00163edf75b7"),
                    NameId = "internekeuken",
                    Title = "Interne Keuken",
                    RadioStation = RadioPlusConst.Radio1
                },
                new RssControllerItem
                {
                    ChannelId = new Guid("dca18fa1-41cf-11e6-aa7a-00163edf843f"),
                    NameId = "plagepreferee",
                    Title = "Plage préférée",
                    RadioStation = RadioPlusConst.Radio2
                },
                new RssControllerItem
                {
                    ChannelId = new Guid("c391a1e7-3b24-11e4-be6c-00163edf75b7"),
                    NameId = "dezoeteinval",
                    Title = "De zoete inval",
                    RadioStation = RadioPlusConst.Radio2
                },
                new RssControllerItem
                {
                    ChannelId = new Guid("a410c7c6-3be5-11e4-be6c-00163edf75b7"),
                    NameId = "derotonde",
                    Title = "De Rotonde",
                    RadioStation = RadioPlusConst.Radio2
                },
                new RssControllerItem
                {
                    ChannelId = new Guid("f2e4071b-34c2-11e8-94b7-02b7b76bf47f"),
                    NameId = "derechtvaardigerechters",
                    Title = "De Rechtvaardige Rechters",
                    RadioStation = RadioPlusConst.Radio2
                },
                new RssControllerItem
                {
                    ChannelId = new Guid("69a0b053-8e90-11e3-b45a-00163edf75b7"),
                    NameId = "nieuwefeiten",
                    Title = "Nieuwe Feiten",
                    RadioStation = RadioPlusConst.Radio1
                }
            };

            _rssItemsDoDownload = rssItemsDoDownload.ToArray();
        }

        [HttpGet]
        public IActionResult GetRaw()
        {
            return Ok(RadioPlusDownloadHandler.DownloadResult);
        }
        [HttpGet]

        public IActionResult Get(string name)
        {
            try
            {
                var entry = new RssManager();

                var rssCollectionToDownload = _rssItemsDoDownload.FirstOrDefault(x => x.NameId?.ToLower() == name?.ToLower());
                var result = rssCollectionToDownload == null ? "Please specify a name" : entry.GetRssString(rssCollectionToDownload.RadioStation, rssCollectionToDownload.Title, rssCollectionToDownload.ChannelId, RadioPlusDownloadHandler.DownloadResult);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}
