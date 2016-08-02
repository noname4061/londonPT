using LondonPublicTransport.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Tfl.Api.Presentation.Entities;

namespace LondonPublicTransport.Controllers
{
    public class BusStopsController : ApiController
    {
        public IEnumerable<StopPoint> Get(double lat, double lng, double radius)
        {
            if (radius <= 0)
                throw new Exception("radius is too small");
            
            var bikePointsUrl = string.Format(@"https://api.tfl.gov.uk/StopPoint?lat={0}&lon={1}&stopTypes={3}&radius={2}",
                lat, lng, radius, "NaptanOnstreetBusCoachStopCluster,NaptanOnstreetBusCoachStopPair,NaptanPrivateBusCoachTram,NaptanPublicBusCoachTram");

            var responseString = TflWebClient.GetResponseString(bikePointsUrl);
            StopPointsResponse placeResponse = JsonConvert.DeserializeObject<StopPointsResponse>(responseString);

            return placeResponse.StopPoints;
        }
    }
}