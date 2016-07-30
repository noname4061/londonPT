using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using Tfl.Api.Presentation.Entities;

namespace LondonPublicTransport.Controllers
{
    public class BikePlacesController : ApiController
    {
        private const string appId = "aaab425f";
        private const string appKey = "62823b9671d9450ff18e851274390e22";

        public IEnumerable<Place> Get()
        {
            var bikePointsUrl = @"https://api.tfl.gov.uk/BikePoint";
            var responseString = GetResponseString(bikePointsUrl);
            List<Place> bikePlaces = JsonConvert.DeserializeObject<List<Place>>(responseString);

            return bikePlaces;
        }

        public IEnumerable<Place> Get(double lat, double lng, double radius)
        {
            var bikePointsUrl = string.Format(@"https://api.tfl.gov.uk/BikePoint?lat={0}&lon={1}&radius={2}", lat, lng, radius);

            var responseString = GetResponseString(bikePointsUrl);
            PlacesResponse placeResponse = JsonConvert.DeserializeObject<PlacesResponse>(responseString);
            
            return placeResponse.Places;
        }
        
        private string GetResponseString(string url)
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