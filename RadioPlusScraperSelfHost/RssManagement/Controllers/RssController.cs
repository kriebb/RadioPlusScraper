using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using WebScrapingProject;

namespace RadioPlusScraperWebApi.Controllers
{
    public class RssController : ApiController
    {
        private readonly RssControllerItem[] _rssItemsDoDownload;

        public RssController()
        {
            var rssItemsDoDownload = new List<RssControllerItem>();
            rssItemsDoDownload.Add(new RssControllerItem() { ChannelId = new Guid("4f2d4613-41d7-11e6-aa7a-00163edf843f"), NameId = "weetikveel", Title = "#WeetIkVeel", RadioStation = "radio1" });
            rssItemsDoDownload.Add(new RssControllerItem() { ChannelId = new Guid("e23e6a65-90d2-11e3-b45a-00163edf75b7"), NameId = "internekeuken", Title = "Interne Keuken", RadioStation = "radio1" });
            _rssItemsDoDownload = rssItemsDoDownload.ToArray();
        }


        public async Task<HttpResponseMessage> Get(string name)
        {
            try
            {
                var entry = new RssManager();

                var rssCollectionToDownload = _rssItemsDoDownload.FirstOrDefault(x => x.NameId?.ToLower() == name?.ToLower());
                var result = rssCollectionToDownload == null ? "Please specify a name" : entry.GetRssString(rssCollectionToDownload.RadioStation, rssCollectionToDownload.Title, rssCollectionToDownload.ChannelId,RadioPlusDownloadHandler.DownloadResult);

                return await Task.FromResult(

                    Request.CreateResponse(HttpStatusCode.OK, result,
                        new XmlMediaTypeFormatter() { Indent = true })
                );
            }
            catch (Exception e)
            {
                return await Task.FromResult(
                    Request.CreateResponse(HttpStatusCode.BadRequest, e.Message + System.Environment.NewLine + e.StackTrace)
                );
            }
        }

    }
}
