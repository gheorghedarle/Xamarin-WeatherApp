using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WeatherApp.Models
{
    public class WeatherModel
    {
        public string icon { get; set; }
        public string description { get; set; }
    }

    public class WeatherDetailsListModel
    {
        public int row { get; set; }
        public int col { get; set; }
        public string label { get; set; }
        public string value { get; set; }
    }

    public class WeatherDetailsModel: BaseModel
    {
        public DateTime dt { get; set; }
        public double temp { get; set; }
        public double maxTemp { get; set; }
        public double minTemp { get; set; }
        public double feels_like { get; set; }
        //public double wind_speed { get; set; }
        //public double wind_deg { get; set; }
        //public double pressure { get; set; }
        //public double humidity { get; set; }
        //public double dew_point { get; set; }
        //public double pop { get; set; }
        //public double uvi { get; set; }
        //public double rain { get; set; }
        //public double visibility { get; set; }
        public List<WeatherDetailsListModel> detailsList { get; set; }
        public WeatherModel weather { get; set; }
        public bool isActive { get; set; }
    }

    public class CurrentWeatherModel
    {
        public WeatherDetailsModel weatherDetails { get; set; }
        public ObservableCollection<WeatherDetailsModel> hourlyWeatherForecast { get; set; }
        public ObservableCollection<WeatherDetailsModel> dailyWeatherForecast { get; set; }
    }
}
