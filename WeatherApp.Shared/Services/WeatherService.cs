using Newtonsoft.Json;

using System;
using System.Threading.Tasks;

using WeatherApp.Core.Models;
using WeatherApp.Core.Models.OpenWeatherMap;

namespace WeatherApp.Core.Services
{
    public class WeatherService
    {
        const string apiKey = "95416e17b99a1d201f157e0ef276134e";
        const string weatherUri = "https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}";

        static WeatherService instance = null;

        public static WeatherService Instance
        {
            get
            {
                if (instance == null)
                    instance = new WeatherService();
                return instance;
            }
        }

        public async Task<WeatherData> GetWeather(double lat, double lon)
        {
            try
            {
                WeatherData weatherData = null;

                string uri = string.Format(weatherUri, lat, lon, apiKey);

                var response = await WebService.Instance.Get(uri);
                if (response != null)
                {
                    string stringContent = await response.Content.ReadAsStringAsync();
                    GetWeatherResponse responseContent = JsonConvert.DeserializeObject<GetWeatherResponse>(stringContent);

                    weatherData = new WeatherData()
                    {
                        Temp = Math.Round(responseContent.Main.Temp - 273.15, 1),
                        Humidity = Math.Round(responseContent.Main.Humidity, 1)
                    };
                }

                return weatherData;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return null;
            }
        }
    }
}
