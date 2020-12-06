using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace WeatherApp.ViewModels
{
    public class WeatherPageViewModel : BaseViewModel
    {
        public string CurrentDate { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentCountry { get; set; }
        public ObservableCollection<string> TodayWeatherList { get; set; }

        public WeatherPageViewModel(INavigationService navigationService): base(navigationService)
        {
            CurrentDate = CreateDateString();
            TodayWeatherList = new ObservableCollection<string>() { 
                "12:00", "13:00", "14:00"
            };
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await GetCurrentLocalityAndCountry();
        }

        private string CreateDateString()
        {
            var currentDate = DateTime.Now;
            return currentDate.ToString("dddd, d MMMM yyyy");
        }

        private async Task GetCurrentLocalityAndCountry()
        {
            try
            {
                CurrentCity = await SecureStorage.GetAsync("currentCity");
                CurrentCountry = await SecureStorage.GetAsync("currentCountry");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
