using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Spatial;
using Newtonsoft.Json;

namespace LondonPublicTransport.Helpers
{
    public static class TransportApiResponseParser
    {
        private const string typeAttributeName = "$type";

        public static List<T> ParseListResponseWithCast<T>(string response)
        {
            var responseObj = ParseResponse(response);
            if (responseObj != null)
            {
                if (responseObj is IEnumerable<object>)
                {
                    return (responseObj as IEnumerable<object>).Cast<T>().ToList();
                }
            }
            else
            {
                throw new Exception("Parse exception: can't read response");
            }

            return null;
        }

        public static T ParseObjectResponseWithCast<T>(string response)
        {
            var responseObj = ParseResponse(response);
            if (responseObj != null)
            {
                if (responseObj is T)
                {
                    return (T)(object)responseObj;
                }
            }
            else
            {
                throw new Exception("Parse exception: can't read response");
            }

            return default(T);
        }

        public static object ParseResponse(string response)
        {
            var objects = JObject.Parse("{objects:" + response + "}");

            if (objects["objects"] is JArray)
            {
                var objectsList = new List<object>();
                foreach (var obj in objects["objects"])
                {
                    objectsList.Add(ParseResponse(obj.ToString()));
                }
                return objectsList;
            }
            else if (objects["objects"] is JObject)
            {
                var objectProperties = (objects["objects"] as JObject).Children().ToList();

                var typeProperty = objectProperties.FirstOrDefault(op => (op as JProperty).Name == typeAttributeName) as JProperty;

                if (typeProperty != null)
                {
                    var objType = Type.GetType(typeProperty.Value.ToString());
                    var objInstance = Activator.CreateInstance(objType);

                    foreach (JProperty objProperty in objectProperties.Where(op => (op as JProperty).Name != typeAttributeName).ToList())
                    {
                        var propertyName = objProperty.Name;
                        var propertyValuePresentation = objProperty.Value;
                        //Potential error due to case mismstch: var property = objType.GetProperty(propertyName);
                        var property = objType.GetProperties().FirstOrDefault(p => p.Name.ToLower() == propertyName.ToLower());

                        if (property != null)
                        {
                            if (propertyValuePresentation is JValue)
                            {
                                var propertyValue = Convert.ChangeType(propertyValuePresentation, property.PropertyType);
                                property.SetValue(objInstance, propertyValue, null);
                            }
                            else if (propertyValuePresentation is JArray)
                            {
                                var propertyValueListPresentation = ParseResponse(propertyValuePresentation.ToString());

                                var collectionPropertyInstanse = Activator.CreateInstance(property.PropertyType) as IList;
                                if ((collectionPropertyInstanse as IEnumerable<object>).Any())
                                {
                                    var collectionItemType = property.PropertyType.GenericTypeArguments.First();

                                    foreach (var propertyValue in propertyValueListPresentation as List<object>)
                                    {
                                        collectionPropertyInstanse.Add(Convert.ChangeType(propertyValue, collectionItemType));
                                    }

                                    property.SetValue(objInstance, collectionPropertyInstanse, null);
                                }
                            }
                            //else if (propertyValuePresentation is JObject)
                            //{
                            //    var propertyValueListPresentationParsed = ParseResponse(propertyValuePresentation.ToString());
                            //    var propertyValue = Convert.ChangeType(propertyValueListPresentationParsed, property.PropertyType);
                            //    property.SetValue(objInstance, propertyValue, null);
                            //}
                        }
                    }

                    return objInstance;
                }
                else
                {
                    throw new Exception("Parse exception: oblect $type missing" + "/n" + objectProperties.First().ToString());
                }
            }

            return null;
        }
    }
}