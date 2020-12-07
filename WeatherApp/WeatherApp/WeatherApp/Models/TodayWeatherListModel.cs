using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Models
{
    public record TodayWeatherListModel
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
