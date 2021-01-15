using System;
using System.Globalization;
using Xamarin.Forms;

namespace WeatherApp.Converters
{
    public class AppThemeIconConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iconCode = (string)value;
            return Application.Current.UserAppTheme.Equals(OSAppTheme.Dark) ? String.Format("icon_{0}_n.png", iconCode) : String.Format("icon_{0}.png", iconCode);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
