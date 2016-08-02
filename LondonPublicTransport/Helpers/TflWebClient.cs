using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace LondonPublicTransport.Helpers
{
    public class TflWebClient
    {
        private const string appId = "aaab425f";
        private const string appKey = "62823b9671d9450ff18e851274390e22";

        public static string GetResponseString(string url)
        {
            using (var wc = new WebClient())
            {
                wc.QueryString.Add("app_id", appId);
                wc.QueryString.Add("app_key", appKey);

                return wc.DownloadString(url);
            }
        }
    }
}