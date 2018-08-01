using System.Diagnostics;
using System.IO;
using WebScrapingProject;

namespace RadioPlusScraperConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            var entry = new RssManager();
            var result = entry.GetRssString("Radio1", "RadioPlus", "Everything from radioPlus");
            File.WriteAllText(@"d:\all.rss.xml",result);
        }
    }
}
