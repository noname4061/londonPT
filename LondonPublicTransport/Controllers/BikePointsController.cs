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
            //var cycleSuperhighway = @"https://api.tfl.gov.uk/CycleSuperhighway";

            List<Place> bikePlaces;

            using (var wc = new WebClient())
            {
                wc.QueryString.Add("app_id", "aaab425f");
                wc.QueryString.Add("app_key", "62823b9671d9450ff18e851274390e22");

                var bikePointsString = wc.DownloadString(bikePointsUrl);
                //ff = bikePointsString;
                //var cycleSuperhighwayString = wc.DownloadString(cycleSuperhighway);

                bikePlaces = JsonConvert.DeserializeObject<List<Place>>(bikePointsString);


                //var dcs = new DataContractJsonSerializer(typeof(List<Place>));
                //var responseObject = dcs.ReadObject(GenerateStreamFromString(bikePointsString));
                //responseObject.ToString();

                //var responseObject1 = TransportApiResponseParser.ParseListResponseWithCast<Place>(bikePointsString);
                //responseObject1.ToString();


                //var responseObject1 = TransportApiResponseParser.ParseListResponseWithCast<CycleSuperhighway>(cycleSuperhighwayString);

                //var dcs1 = new DataContractSerializer(typeof(List<CycleSuperhighway>));
                //var dcs1 = new DataContractJsonSerializer(typeof(List<CycleSuperhighway>));
                //var responseObject1 = dcs1.ReadObject(GenerateStreamFromString(cycleSuperhighwayString));
                //responseObject1.ToString();
            }

            return bikePlaces;
        }
    }
}