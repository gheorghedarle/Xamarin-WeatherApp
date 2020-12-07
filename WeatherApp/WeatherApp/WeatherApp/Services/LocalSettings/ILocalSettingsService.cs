using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Services.LocalSettings
{
    public interface ILocalSettingsService
    {
        (string WeatherApiBaseUrl, string WeatherApiKey) LoadLocalSettings();
    }
}
