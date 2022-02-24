using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
                        temp = Convert.ToDouble(json["current"]["temp"].ToString()),
                        feels_like = Convert.ToDouble(json["current"]["feels_like"].ToString()),
                        weather = new WeatherModel()
                        {
                            icon = json["current"]["weather"][0]["icon"].ToString(),
                            description = json["current"]["weather"][0]["description"].ToString()
                        },
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
                    detailsList = new List<WeatherDetailsListModel>()
                    {
                        new WeatherDetailsListModel() { row = 0, col = 0, label = "Rain:", value = $"{Math.Round(Convert.ToDouble(hourly[i]["pop"].ToString()) * 100, 1)}%"},
                        new WeatherDetailsListModel() { row = 0, col = 1, label = "Pressure", value = $"{Math.Round(Convert.ToDouble(hourly[i]["pressure"].ToString()), 1)}hPa"},
                        new WeatherDetailsListModel() { row = 1, col = 0, label = "Humidity:", value = $"{Math.Round(Convert.ToDouble(hourly[i]["humidity"].ToString()), 1)}%"},
                        new WeatherDetailsListModel() { row = 1, col = 1, label = "Visibility", value = $"{Math.Round(Convert.ToDouble(hourly[i]["visibility"].ToString()) / 1000, 1)} km"},
                        new WeatherDetailsListModel() { row = 2, col = 0, label = "Wind speed:", value = $"{Math.Round(Convert.ToDouble(hourly[i]["wind_speed"].ToString()), 1)} m/s"}
                    },
                    weather = new WeatherModel()
                    {
                        icon = hourly[i]["weather"][0]["icon"].ToString(),
                        description = hourly[i]["weather"][0]["description"].ToString()
                    },
                });
            }
            return hourlyForecast;
        }

        private ObservableCollection<WeatherDetailsModel> CreateDailyWeatherForecast(JToken daily)
        {
            var dailyForecast = new ObservableCollection<WeatherDetailsModel>();
            for (var i = 1; i <= 7; i++)
            {
                dailyForecast.Add(new WeatherDetailsModel()
                {
                    dt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(daily[i]["dt"])).LocalDateTime,
                    temp = Convert.ToDouble(daily[i]["temp"]["day"].ToString()),
                    minTemp = Convert.ToDouble(daily[i]["temp"]["min"].ToString()),
                    maxTemp = Convert.ToDouble(daily[i]["temp"]["max"].ToString()),
                    detailsList = new List<WeatherDetailsListModel>()
                    {
                        new WeatherDetailsListModel() { row = 0, col = 0, label = "Rain:", value = $"{Math.Round(Convert.ToDouble(daily[i]["pop"].ToString()) * 100, 1)}%"},
                        new WeatherDetailsListModel() { row = 0, col = 1, label = "Pressure", value = $"{Math.Round(Convert.ToDouble(daily[i]["pressure"].ToString()), 1)}hPa"},
                        new WeatherDetailsListModel() { row = 1, col = 0, label = "Humidity:", value = $"{Math.Round(Convert.ToDouble(daily[i]["humidity"].ToString()), 1)}%"},
                        new WeatherDetailsListModel() { row = 1, col = 1, label = "Clouds", value = $"{Convert.ToDouble(daily[i]["clouds"].ToString())}%"},
                        new WeatherDetailsListModel() { row = 2, col = 0, label = "Wind speed:", value = $"{Math.Round(Convert.ToDouble(daily[i]["wind_speed"].ToString()), 1)} m/s"},
                        new WeatherDetailsListModel() { row = 2, col = 1, label = "UV Index:", value = $"{Math.Round(Convert.ToDouble(daily[i]["uvi"].ToString()), 1)}"},
                        new WeatherDetailsListModel() { row = 3, col = 0, label = "Sunrise:", value = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(daily[i]["sunrise"])).LocalDateTime.ToString("HH:mm") },
                        new WeatherDetailsListModel() { row = 3, col = 1, label = "Sunset:", value = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(daily[i]["sunset"])).LocalDateTime.ToString("HH:mm")}
                    },
                    weather = new WeatherModel()
                    {
                        icon = daily[i]["weather"][0]["icon"].ToString(),
                        description = daily[i]["weather"][0]["description"].ToString()
                    },
                });
            }
            return dailyForecast;
        }
    }
}
