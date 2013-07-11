using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Umbraco.Web.BaseRest;

namespace Skybrud.Umbraco
{
    [RestExtension("FetchFeed")]
    public class FetchFeed
    {
        [RestExtensionMethod(AllowAll = true, ReturnXml = false)]
        public static void Update()
        {
            var config = FetchFeedConfiguration.Configuration();

            foreach (var feed in config.Feeds)
            {
                var path = HttpContext.Current.Server.MapPath(feed.Path);

                var lastUpdated = DateTime.MinValue;
                if (File.Exists(path))
                {
                    lastUpdated = File.GetLastWriteTime(path);
                }
                else
                {
                    File.WriteAllText(path, " ", Encoding.Default);
                }

                var timeSpan = DateTime.Now.Subtract(lastUpdated).TotalMinutes;
                if (timeSpan > feed.Interval)
                {
                    var data = File.ReadAllText(path, Encoding.Default);

                    try
                    {
                        using (var webClient = new WebClient())
                        {
                            data = webClient.DownloadString(feed.Url);
                        }
                    }
                    catch
                    {
                    }

                    File.WriteAllText(path, data, Encoding.Default);
                }
            }
        }
    }
}
