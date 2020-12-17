using System;
using System.Collections.ObjectModel;

namespace WeatherApp.Models
{
    public class WeatherModel
    {
        public string icon { get; set; }
        public string description { get; set; }
    }

    public class HourlyModel: BaseModel
    {
        public DateTime dt { get; set; }
        public double temp { get; set; }
        public WeatherModel weather { get; set; }
        public bool isActive { get; set; }
    }

    public class CurrentWeatherModel
    {
        public DateTime dt { get; set; }
        public DateTime sunrise { get; set; }
        public DateTime sunset { get; set; }
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double wind_speed { get; set; }
        public double wind_deg { get; set; }
        public double humidity { get; set; }
        public double rain { get; set; }
        public WeatherModel weather { get; set; }
        public ObservableCollection<HourlyModel> hourlyWeatherForecast { get; set; }
    }
}
