using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Skybrud.Umbraco.Extensions;
using Umbraco.Core.Logging;

namespace Skybrud.Umbraco {
    
    public class FetchFeedConfiguration {
        
        public List<Feed> Feeds { get; private set; }

        private FetchFeedConfiguration() {
            string configPath = HttpContext.Current.Server.MapPath("~/config/FetchFeed.config");
            Feeds = new List<Feed>();
            if (File.Exists(configPath)) FetchFeedLoadConfiguration(configPath);
        }

        private void FetchFeedLoadConfiguration(string configPath) {
            
            // Parse the XML file
            XDocument doc = XDocument.Load(configPath);

            // Get the root element
            XElement feeds = doc.Descendants("feeds").FirstOrDefault();
            if (feeds == null) return;

            // Clear the list of feeds (shouldn't really be necessary anymore)
            Feeds.Clear();

            // Iterate through each "feed" element
            foreach (XElement xFeed in feeds.Descendants("feed")) {

                try {

                    string alias = xFeed.GetAttributeValue("alias") ?? "";
                    string url = xFeed.GetAttributeValue("url") ?? "";
                    string path = xFeed.GetAttributeValue("path") ?? "";
                    string interval = xFeed.GetAttributeValue("interval") ?? "";
                    string encoding = (xFeed.GetAttributeValue("encoding") ?? "").ToLower();

                    if (alias != "" && url != "" && path != "" && interval != "") {

                        Feed feed = new Feed {
                            Alias = alias,
                            Url = url,
                            Path = path,
                            Interval = Double.Parse(interval)
                        };

                        switch (encoding) {
                            case "utf-8":
                            case "utf8":
                                feed.Encoding = Encoding.UTF8;
                                break;
                        }

                        Feeds.Add(feed);

                    }
                
                } catch (Exception ex) {

                    LogHelper.Error<FetchFeedConfiguration>("Unable to parse feed element in ~/config/FetchFeed.config\r\n\r\n" + xFeed, ex);

                }

            }
        
        }

        private static FetchFeedConfiguration _instance;

        public static FetchFeedConfiguration Configuration() {
            return _instance ?? (_instance = new FetchFeedConfiguration());
        }
    
    }

}