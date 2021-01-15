using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services.Weather
{
    public interface IWeatherService
    {
        Task<CurrentWeatherModel> GetCurrentWeatherAndHourlyForecastByLatLon(double lat, double lon, string units);
    }
}
