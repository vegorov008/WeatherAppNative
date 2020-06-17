using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Core.Models.OpenWeatherMap
{
    //{
    //    "coord":
    //    {
    //        "lon":139.01,
    //        "lat":35.02
    //    },
    //    "weather":
    //    [
    //        {
    //            "id":800,
    //            "main":"Clear",
    //            "description":"clear sky",
    //            "icon":"01n"
    //        }
    //    ],
    //    "base":"stations",
    //    "main":
    //    {
    //        "temp":285.514,
    //        "pressure":1013.75,
    //        "humidity":100,
    //        "temp_min":285.514,
    //        "temp_max":285.514,
    //        "sea_level":1023.22,
    //        "grnd_level":1013.75
    //    },
    //    "wind":
    //    {
    //        "speed":5.52,
    //        "deg":311
    //    },
    //    "clouds":
    //    {
    //        "all":0
    //    },
    //    "dt":1485792967,
    //    "sys":
    //    {
    //        "message":0.0025,
    //        "country":"JP",
    //        "sunrise":1485726240,
    //        "sunset":1485763863
    //    },
    //    "id":1907296,
    //    "name":"Tawarano",
    //    "cod":200
    //}

    public class GetWeatherResponse
    {
        public GetWeatherResponse()
        {

        }

        [JsonProperty(PropertyName = "coord")]
        public Coordinates Coordinates { get; set; }

        [JsonProperty(PropertyName = "weather")]
        public List<Weather> Weathers { get; set; }

        [JsonProperty(PropertyName = "main")]
        public Main Main { get; set; }



        [JsonProperty(PropertyName = "base")]
        public string Base { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "cod")]
        public string Cod { get; set; }
    }
}
