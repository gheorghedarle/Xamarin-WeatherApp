using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WeatherApp.Core;
using WeatherApp.Models;
using Xamarin.Essentials;

namespace WeatherApp.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClientFactory _httpClientFactory;

        public WeatherService(HttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CurrentWeatherModel> GetCurrentWeatherAndHourlyForecastByLatLon(double lat, double lon)
        {
            var baseUrl = await SecureStorage.GetAsync("weatherApiBaseUrl");
            var apiKey = await SecureStorage.GetAsync("weatherApiKey");
            var path = "onecall";
            var response = await _httpClientFactory.GetHttpClient().GetAsync($"{baseUrl}/{path}?lat={lat}&lon={lon}&units=metric&exclude=minutely,daily,alerts&appid={apiKey}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(result);
                var hourlyForecast = CreateHourlyForecast(json["hourly"]);
                return new CurrentWeatherModel()
                {
                    dt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(json["current"]["dt"])).LocalDateTime,
                    sunrise = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(json["current"]["sunrise"])).LocalDateTime,
                    sunset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(json["current"]["sunset"])).LocalDateTime,
                    temp = Convert.ToDouble(json["current"]["temp"].ToString()),
                    feels_like = Convert.ToDouble(json["current"]["feels_like"].ToString()),
                    humidity = Convert.ToDouble(json["current"]["humidity"].ToString()),
                    wind_speed = Convert.ToDouble(json["current"]["wind_speed"].ToString()),
                    wind_deg = Convert.ToDouble(json["current"]["wind_deg"].ToString()),
                    weather = new WeatherModel()
                    {
                        icon = json["current"]["weather"][0]["icon"].ToString(),
                        description = json["current"]["weather"][0]["description"].ToString()
                    },
                    rain = json["current"]["rain"] != null ? Convert.ToDouble(json["current"]["rain"]["1h"].ToString()) : 0,
                    hourlyWeatherForecast = hourlyForecast
                };
            }
            return null;
        }

        private ObservableCollection<HourlyModel> CreateHourlyForecast(JToken hourly)
        {
            var hourlyForecast = new ObservableCollection<HourlyModel>();
            for (var i = 0; i < 48; i++)
            {
                hourlyForecast.Add(new HourlyModel()
                {
                    dt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(hourly[i]["dt"])).LocalDateTime,
                    temp = Convert.ToDouble(hourly[i]["temp"].ToString()),
                    weather = new WeatherModel()
                    {
                        icon = hourly[i]["weather"][0]["icon"].ToString(),
                        description = hourly[i]["weather"][0]["description"].ToString()
                    },
                });
            }
            return hourlyForecast;
        }
    }
}
