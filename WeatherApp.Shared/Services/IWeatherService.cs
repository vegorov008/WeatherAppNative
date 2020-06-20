using System.Threading.Tasks;
using WeatherApp.Core.Models;

namespace WeatherApp.Shared.Services
{
    public interface IWeatherService
    {
        Task<WeatherData> GetWeather(double lat, double lon);
    }
}
