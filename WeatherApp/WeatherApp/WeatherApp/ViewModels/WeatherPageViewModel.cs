using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WeatherApp.Models;
using Xamarin.Essentials;

namespace WeatherApp.ViewModels
{
    public class WeatherPageViewModel : BaseViewModel
    {
        public string CurrentDate { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentCountry { get; set; }
        public ObservableCollection<TodayWeatherListModel> TodayWeatherList { get; set; }

        public WeatherPageViewModel(INavigationService navigationService): base(navigationService)
        {
            CurrentDate = CreateDateString();
            TodayWeatherList = new ObservableCollection<TodayWeatherListModel>() {
                new TodayWeatherListModel("NOW", "02d", "23"),
                new TodayWeatherListModel("12:00", "01d", "23"),
                new TodayWeatherListModel("13:00", "01d", "24"),
                new TodayWeatherListModel("14:00", "02d", "25"),
                new TodayWeatherListModel("15:00", "02d", "27"),
                new TodayWeatherListModel("16:00", "02d", "26"),
                new TodayWeatherListModel("17:00", "03d", "20"),
                new TodayWeatherListModel("18:00", "03d", "26"),
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
