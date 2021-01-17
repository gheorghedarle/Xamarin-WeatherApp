using System;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.Converters
{
    public class UnitConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double)value;
            var param = (string)parameter;
            var unit = Preferences.Get("units", "metric");
            switch(parameter)
            {
                case "temperature": return unit.Equals("metric") ? $"{Math.Round(val)}°C" : $"{Math.Round(val)}°F";
                case "speed": return unit.Equals("metric") ? $"{Math.Round(val, 1)} m/s" : $"{Math.Round(val, 1)} mph";
                case "sign": return unit.Equals("metric") ? "C" : "F";
                default: return Math.Round(val, 1);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
