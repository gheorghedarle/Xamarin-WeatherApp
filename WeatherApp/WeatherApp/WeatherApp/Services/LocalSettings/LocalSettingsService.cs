using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WeatherApp.Services.LocalSettings
{
    public class LocalSettingsService : ILocalSettingsService
    {
        public (string WeatherApiBaseUrl, string WeatherApiKey) LoadLocalSettings()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resName = assembly.GetManifestResourceNames()?.FirstOrDefault(r => r.EndsWith("settings.json", StringComparison.OrdinalIgnoreCase));

            using var file = assembly.GetManifestResourceStream(resName);
            using var sr = new StreamReader(file);

            var json = sr.ReadToEnd();
            var j = JObject.Parse(json);

            var weatherApiBaseUrl = j.Value<string>("openWeatherMapApiBaseUrl");
            var weatherApiKey = j.Value<string>("openWeatherMapApiKey");

            return (
                WeatherApiBaseUrl: weatherApiBaseUrl,
                WeatherApiKey: weatherApiKey
            );
        }
    }
}
