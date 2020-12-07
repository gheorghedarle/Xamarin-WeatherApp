using System;

namespace WeatherApp.Models
{
    public record WeatherModel
    {
        public string icon { get; set; }
        public string description { get; set; }
    }

    public record CurrentWeatherModel
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
    }
}
