namespace WeatherApp.Services.LocalSettings
{
    public interface ILocalSettingsService
    {
        (string WeatherApiBaseUrl, string WeatherApiKey) LoadLocalSettings();
    }
}
