using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Skybrud.Umbraco
{
    class FetchFeedConfiguration
    {
        public List<Feed> Feeds { get; private set; }

        private FetchFeedConfiguration()
        {
            var configPath = HttpContext.Current.Server.MapPath("~/config/FetchFeed.config");

            Feeds = new List<Feed>();
            Feeds.Add(new Feed() { Alias = "skybrud", Url = "http://www.skybrud.dk/rss", Path = "~/App_Data/skybrud.xml", Interval = 10 });

            if (File.Exists(configPath))
                FetchFeedLoadConfiguration(configPath);
        }

        private void FetchFeedLoadConfiguration(string configPath)
        {
            XDocument doc = XDocument.Load(configPath);

            var feeds = doc.Descendants("feeds").FirstOrDefault();
            if (feeds != null)
            {
                Feeds.Clear();

                foreach (var feed in feeds.Descendants("feed"))
                {
                    var alias = feed.Attribute("alias").Value;
                    var url = feed.Attribute("url").Value;
                    var path = feed.Attribute("path").Value;
                    var interval = feed.Attribute("interval").Value;

                    if (alias != "" && url != "" && path != "" && interval != "")
                    {
                        try
                        {
                            var intervalValue = double.Parse(interval);
                            Feeds.Add(new Feed() { Alias = alias, Url = url, Path = path, Interval = intervalValue });
                        }
                        catch
                        {
                        }
                    }

                }
            }
        }

        private static FetchFeedConfiguration _instance;

        public static FetchFeedConfiguration Configuration()
        {
            return _instance ?? (_instance = new FetchFeedConfiguration());
        }
    }
}
