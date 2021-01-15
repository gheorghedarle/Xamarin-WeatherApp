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

        public async Task<CurrentWeatherModel> GetCurrentWeatherAndHourlyForecastByLatLon(double lat, double lon, string units)
        {
            var baseUrl = await SecureStorage.GetAsync("weatherApiBaseUrl");
            var apiKey = await SecureStorage.GetAsync("weatherApiKey");
            var path = "onecall";
            var response = await _httpClientFactory.GetHttpClient().GetAsync($"{baseUrl}/{path}?lat={lat}&lon={lon}&units={units}&exclude=minutely,alerts&appid={apiKey}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(result);
                var hourlyWeatherForecast = CreateHourlyWeatherForecast(json["hourly"]);
                var dailyWeatherForecast = CreateDailyWeatherForecast(json["daily"]);
                return new CurrentWeatherModel()
                {
                    weatherDetails = new WeatherDetailsModel()
                    {
                        dt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(json["current"]["dt"])).LocalDateTime,
                        sunrise = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(json["current"]["sunrise"])).LocalDateTime,
                        sunset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(json["current"]["sunset"])).LocalDateTime,
                        temp = Convert.ToDouble(json["current"]["temp"].ToString()),
                        feels_like = Convert.ToDouble(json["current"]["feels_like"].ToString()),
                        humidity = Convert.ToDouble(json["current"]["humidity"].ToString()),
                        wind_speed = Convert.ToDouble(json["current"]["wind_speed"].ToString()),
                        wind_deg = Convert.ToDouble(json["current"]["wind_deg"].ToString()),
                        pressure = Convert.ToDouble(json["current"]["pressure"].ToString()),
                        dew_point = Convert.ToDouble(json["current"]["dew_point"].ToString()),
                        uvi = Convert.ToDouble(json["current"]["uvi"].ToString()),
                        weather = new WeatherModel()
                        {
                            icon = json["current"]["weather"][0]["icon"].ToString(),
                            description = json["current"]["weather"][0]["description"].ToString()
                        },
                        rain = json["current"]["rain"] != null ? Convert.ToDouble(json["current"]["rain"]["1h"].ToString()) : 0,
                    },
                    hourlyWeatherForecast = hourlyWeatherForecast,
                    dailyWeatherForecast = dailyWeatherForecast
                };
            }
            return null;
        }

        private ObservableCollection<WeatherDetailsModel> CreateHourlyWeatherForecast(JToken hourly)
        {
            var hourlyForecast = new ObservableCollection<WeatherDetailsModel>();
            for (var i = 0; i < 24; i++)
            {
                hourlyForecast.Add(new WeatherDetailsModel()
                {
                    dt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(hourly[i]["dt"])).LocalDateTime,
                    temp = Convert.ToDouble(hourly[i]["temp"].ToString()),
                    feels_like = Convert.ToDouble(hourly[i]["feels_like"].ToString()),
                    humidity = Convert.ToDouble(hourly[i]["humidity"].ToString()),
                    wind_speed = Convert.ToDouble(hourly[i]["wind_speed"].ToString()),
                    wind_deg = Convert.ToDouble(hourly[i]["wind_deg"].ToString()),
                    visibility = Convert.ToDouble(hourly[i]["visibility"].ToString()) / 1000,
                    pressure = Convert.ToDouble(hourly[i]["pressure"].ToString()),
                    weather = new WeatherModel()
                    {
                        icon = hourly[i]["weather"][0]["icon"].ToString(),
                        description = hourly[i]["weather"][0]["description"].ToString()
                    },
                    rain = hourly[i]["rain"] != null ? Convert.ToDouble(hourly[i]["rain"]["1h"].ToString()) : 0,
                });
            }
            return hourlyForecast;
        }

        private ObservableCollection<WeatherDetailsModel> CreateDailyWeatherForecast(JToken daily)
        {
            var hourlyForecast = new ObservableCollection<WeatherDetailsModel>();
            for (var i = 1; i < 7; i++)
            {
                hourlyForecast.Add(new WeatherDetailsModel()
                {
                    dt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(daily[i]["dt"])).LocalDateTime,
                    sunrise = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(daily[i]["sunrise"])).LocalDateTime,
                    sunset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(daily[i]["sunset"])).LocalDateTime,
                    temp = Convert.ToDouble(daily[i]["temp"]["day"].ToString()),
                    minTemp = Convert.ToDouble(daily[i]["temp"]["min"].ToString()),
                    maxTemp = Convert.ToDouble(daily[i]["temp"]["max"].ToString()),
                    feels_like = Convert.ToDouble(daily[i]["feels_like"]["day"].ToString()),
                    humidity = Convert.ToDouble(daily[i]["humidity"].ToString()),
                    wind_speed = Convert.ToDouble(daily[i]["wind_speed"].ToString()),
                    wind_deg = Convert.ToDouble(daily[i]["wind_deg"].ToString()),
                    pressure = Convert.ToDouble(daily[i]["pressure"].ToString()),
                    dew_point = Convert.ToDouble(daily[i]["dew_point"].ToString()),
                    pop = Convert.ToDouble(daily[i]["pop"].ToString()) * 100,
                    uvi = Convert.ToDouble(daily[i]["uvi"].ToString()),
                    weather = new WeatherModel()
                    {
                        icon = daily[i]["weather"][0]["icon"].ToString(),
                        description = daily[i]["weather"][0]["description"].ToString()
                    },
                });
            }
            return hourlyForecast;
        }
    }
}
