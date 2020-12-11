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
        public bool IsRefreshing { get; set; }
        public DelegateCommand RefreshCommand { get; set; }

        public WeatherPageViewModel(
            INavigationService navigationService,
            IWeatherService weatherService): base(navigationService)
        {
            _weatherService = weatherService;

            CurrentDate = CreateDateString();
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
