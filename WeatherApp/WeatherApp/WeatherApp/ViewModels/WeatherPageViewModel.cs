using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services.Weather;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;

namespace WeatherApp.ViewModels
{
    public class WeatherPageViewModel : BaseViewModel
    {
        private readonly IWeatherService _weatherService;

        private double _currentLat;
        private double _currentLon;

        public string CurrentDate { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentCountry { get; set; }
        public CurrentWeatherModel CurrentWeather { get; set; }
        public ObservableCollection<TodayWeatherListModel> TodayWeatherList { get; set; }
        public bool IsRefreshing { get; set; }
        public DelegateCommand RefreshCommand { get; set; }

        public WeatherPageViewModel(
            INavigationService navigationService,
            IWeatherService weatherService): base(navigationService)
        {
            _weatherService = weatherService;

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
            RefreshCommand = new DelegateCommand(RefreshCommandHandler);

            MainState = LayoutState.Loading;
        }

        private async void RefreshCommandHandler()
        {
            IsRefreshing = true;
            await GetCurrentWeather();
            IsRefreshing = false;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await GetCurrentLocalityAndCountry();
            await GetCurrentLatLon();
            await GetCurrentWeather();
            MainState = LayoutState.None;
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

        private async Task GetCurrentLatLon()
        {
            try
            {
                var currentLat = await SecureStorage.GetAsync("currentLatitude");
                var currentLon = await SecureStorage.GetAsync("currentLongitude");
                _currentLat = Convert.ToDouble(currentLat);
                _currentLon = Convert.ToDouble(currentLon);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task GetCurrentWeather()
        {
            CurrentWeather = await _weatherService.GetCurrentWeatherAndHourlyForecastByLatLon(_currentLat, _currentLon);
        }
    }
}
