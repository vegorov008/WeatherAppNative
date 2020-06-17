using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Core.Models.OpenWeatherMap
{
    public class Weather
    {
        //    "id":800,
        //    "main":"Clear",
        //    "description":"clear sky",
        //    "icon":"01n"

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "main")]
        public string Main { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }
    }
}
