using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using Skybrud.WebApi.Json;
using Umbraco.Core.Logging;
using Umbraco.Web.WebApi;

namespace Skybrud.Umbraco.Controllers.Api {
    
    [JsonOnlyConfiguration]
    public class FetchFeedController : UmbracoApiController {

        protected HttpServerUtility Server {
            get { return HttpContext.Current.Server; }
        }

        [HttpGet]
        public object Update() {

            // Load the configuration
            FetchFeedConfiguration config = FetchFeedConfiguration.Configuration();

            int success = 0;
            int failed = 0;

            // Iterate over the feeds specified in the configuration
            foreach (Feed feed in config.Feeds) {

                bool succeeded = false;

                try {
                    
                    // Map the path to the destination file
                    string path = HttpContext.Current.Server.MapPath(feed.Path);

                    // Create the destination directory (if it doesn't already exist)
                    string directory = Path.GetDirectoryName(path);
                    if (directory == null) continue;
                    Directory.CreateDirectory(directory);

                    // Get the timestamp for when the file was last updated (or create a new empty file if it doesn't already exist)
                    DateTime lastUpdated = DateTime.MinValue;
                    if (File.Exists(path)) {
                        lastUpdated = File.GetLastWriteTime(path);
                    } else {
                        File.WriteAllText(path, " ", Encoding.Default);
                    }

                    // Skip downloading the feed since it has been updated within the interval
                    if (DateTime.Now.Subtract(lastUpdated).TotalMinutes <= feed.Interval) continue;

                    // Read the contents of the file
                    string data = File.ReadAllText(path, Encoding.Default);

                    try {

                        // Download the feed from the URL
                        using (WebClient webClient = new WebClient()) {
                            data = webClient.DownloadString(feed.Url);
                        }
                
                    } catch (Exception ex) {
                    
                        // Append the error to the Umbraco log and increment the "failed" counter
                        LogHelper.Error<FetchFeedController>("Unable to download feed from URL: " + feed.Url, ex);

                    }

                    // Write the data to the file
                    File.WriteAllText(path, data, Encoding.Default);

                    succeeded = true;

                } catch (Exception ex) {

                    // Append the error to the Umbraco log and increment the "failed" counter
                    LogHelper.Error<FetchFeedController>("Unable to download feed from URL: " + feed.Url, ex);

                }

                if (succeeded) {
                    success++;
                } else {
                    failed++;
                }
            
            }

            return new {
                data = new {
                    success,
                    failed
                }
            };

        }

    }

}