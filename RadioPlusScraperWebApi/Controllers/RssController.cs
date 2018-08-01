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
        private const string ChannelWeetIkVeel = "4f2d4613-41d7-11e6-aa7a-00163edf843f";
        private const string ChannelInterneKeuken = "e23e6a65-90d2-11e3-b45a-00163edf75b7";

        private const string TitleWeetIkVeel = "#WeetIkVeel";
        private const string TitleInterneKeuken = "Interne Keuken";

        private const string Radio1 = "radio1";

        public async Task<HttpResponseMessage> Get(string name)
        {
            try
            {
                // parse query parameter             
                var entry = new RssManager();

                string result;
                if (string.Compare("weetikveel".ToLower(), name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = entry.GetRssString(Radio1, ChannelWeetIkVeel, TitleWeetIkVeel);
                }
                else
                {
                    if (string.Compare("internekeuken".ToLower(), name, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = entry.GetRssString(Radio1, ChannelInterneKeuken, TitleInterneKeuken);
                    }
                    else
                    {
                        result = entry.GetRssString(Radio1, "RadioPlus", "Everything from RadioPlus");
                    }

                }

                return await Task.FromResult(

                    Request.CreateResponse(HttpStatusCode.OK, result,
                        new XmlMediaTypeFormatter() { Indent = true })
                );
            }
            catch (Exception e)
            {
                return await Task.FromResult(
                    Request.CreateResponse(HttpStatusCode.BadRequest, e.Message)
                );
            }
        }
        public async Task<HttpResponseMessage> Get()
        {
            return await Get("");
        }
    }
}
