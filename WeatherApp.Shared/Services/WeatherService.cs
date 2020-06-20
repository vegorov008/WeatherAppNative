using Newtonsoft.Json;

using System;
using System.Threading.Tasks;

using WeatherApp.Core.Models;
using WeatherApp.Core.Models.OpenWeatherMap;
using WeatherApp.Shared.Services;

namespace WeatherApp.Core.Services
{
    public class WeatherService : IWeatherService
    {
        const string apiKey = "95416e17b99a1d201f157e0ef276134e";
        const string weatherUri = "https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}";

        public async Task<WeatherData> GetWeather(double lat, double lon)
        {
            WeatherData weatherData = null;

            try
            {
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
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }

            return weatherData;
        }
    }
}
