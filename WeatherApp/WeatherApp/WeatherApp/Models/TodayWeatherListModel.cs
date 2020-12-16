using System;

namespace WeatherApp.Models
{
    public class TodayWeatherListModel
    {
        public string Hour { get; }
        public string Temperature { get; }
        public string Icon { get; }

        public TodayWeatherListModel(string hour, string icon, string temperature)
        {
            Hour = hour;
            Icon = String.Format("icon_{0}.png", icon);
            Temperature = temperature;
        }
    }
}
