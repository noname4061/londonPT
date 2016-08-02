using LondonPublicTransport.Helpers;
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
        public IEnumerable<Place> Get()
        {
            var bikePointsUrl = @"https://api.tfl.gov.uk/BikePoint";
            var responseString = TflWebClient.GetResponseString(bikePointsUrl);
            List<Place> bikePlaces = JsonConvert.DeserializeObject<List<Place>>(responseString);

            return bikePlaces;
        }

        public IEnumerable<Place> Get(double lat, double lng, double radius)
        {
            if (radius <= 0)
                return Get();

            var bikePointsUrl = string.Format(@"https://api.tfl.gov.uk/BikePoint?lat={0}&lon={1}&radius={2}", lat, lng, radius);

            var responseString = TflWebClient.GetResponseString(bikePointsUrl);
            PlacesResponse placeResponse = JsonConvert.DeserializeObject<PlacesResponse>(responseString);
            
            return placeResponse.Places;
        }
    }
}