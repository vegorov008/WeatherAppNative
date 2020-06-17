using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Core.Models.OpenWeatherMap
{
    public class Coordinates
    {
        [JsonProperty(PropertyName = "lat")]
        public double Lat { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public double Lon { get; set; }
    }
}
