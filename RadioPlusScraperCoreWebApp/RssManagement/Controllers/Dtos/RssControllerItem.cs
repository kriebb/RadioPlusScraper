using System;

namespace RadioPlusScraperCoreWebApp.RssManagement.Controllers.Dtos
{
    public class RssControllerItem
    {
        public string NameId { get; set; }
        public Guid ChannelId { get; set; }
        public string Title { get; set; }
        public string RadioStation { get; set; }
    }
}