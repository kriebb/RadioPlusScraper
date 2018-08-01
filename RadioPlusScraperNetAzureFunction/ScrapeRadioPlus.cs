using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using WebScrapingProject;

namespace RadioPlusScraperNetAzureFunction
{
    public static class ScrapeRadioPlus
    {
        private const string ChannelWeetIkVeel = "4f2d4613-41d7-11e6-aa7a-00163edf843f";
        private const string ChannelInterneKeuken = "e23e6a65-90d2-11e3-b45a-00163edf75b7";

        private const string TitleWeetIkVeel = "#WeetIkVeel";
        private const string TitleInterneKeuken = "Interne Keuken";

        private const string Radio1 = "radio1";


        [FunctionName("ScrapeRadioPlus")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            try
            {
                // parse query parameter
                string name = req.GetQueryNameValuePairs()
                    .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                    .Value;

                if (name == null)
                {
                    // Get request body
                    dynamic data = await req.Content.ReadAsAsync<object>();
                    name = data?.name;
                }
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

                return req.CreateResponse(HttpStatusCode.OK, result, new XmlMediaTypeFormatter() { Indent = true });
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                return req.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }


        }
    }
}
