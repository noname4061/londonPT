using LondonPublicTransport.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using Tfl.Api.Presentation.Entities;

namespace LondonPublicTransport.Controllers
{
    public class StopPointArrivalsController : ApiController
    {
        public string Get(string ids)
        {
            var idArray = ids.Split(',');
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            
            var arrivals = new List<Prediction>();
            foreach (var id in idArray)
            {
                var bikePointsUrl = string.Format(@"https://api.tfl.gov.uk/StopPoint/{0}/Arrivals", id);
                var responseString = TflWebClient.GetResponseString(bikePointsUrl);
                arrivals.AddRange(JsonConvert.DeserializeObject<List<Prediction>>(responseString));
            }

            if (arrivals.Count <= 0)
                return "";

            var html = new StringBuilder();

            html.Append(@"<h3>&nbsp&nbsp&nbsp&nbsp&quot" + arrivals.First().StationName + @"&quot</h3>");

            foreach (var platform in arrivals.Select(a => a.PlatformName).Distinct())
            {
                html.Append("<b>&nbsp&nbsp&nbsp&nbspPlatform " + platform + "</b><br/>");

                var platformArrivals = arrivals.Where(a => a.PlatformName == platform);
                foreach (var number in platformArrivals.Select(ap => ap.LineId).Distinct().OrderBy(id => id))
                {
                    html.Append(string.Format("<p><b>&nbsp{0}</b><br/>", number));

                    var first = platformArrivals.FirstOrDefault(pa => pa.LineId == number);
                    html.Append("&nbsptowards: " + first.Towards + "<br/>");
                    html.Append("&nbspdestination: " + first.DestinationName + "<br/>");
                    html.Append("&nbsp" + string.Join(", ", platformArrivals.Where(pa => pa.LineId == number).Select(pa => pa.ExpectedArrival).OrderBy(pa => pa).Select(pa => pa.ToString("hh:mm")).ToArray()) + "</p><hr/>");
                }
                html.Append("<br/>");
            }

            return html.ToString();
        }
    }
}