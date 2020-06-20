using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Core.Models.OpenWeatherMap
{
    public class Main
    {
        //  "temp":285.514,
        //  "pressure":1013.75,
        //  "humidity":100,
        //  "temp_min":285.514,
        //  "temp_max":285.514,
        //  "sea_level":1023.22,
        //  "grnd_level":1013.75

        [JsonProperty(PropertyName = "temp")]
        public double Temp { get; set; }

        [JsonProperty(PropertyName = "pressure")]
        public double Pressure { get; set; }

        [JsonProperty(PropertyName = "humidity")]
        public double Humidity { get; set; }

        [JsonProperty(PropertyName = "temp_min")]
        public double TempMin { get; set; }

        [JsonProperty(PropertyName = "temp_max")]
        public double TempMax { get; set; }

        [JsonProperty(PropertyName = "sea_level")]
        public double SeaLevel { get; set; }

        [JsonProperty(PropertyName = "grnd_level")]
        public double GroundLevel { get; set; }
    }
}
