using Newtonsoft.Json;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Events;
using WeatherApp.Models;
using WeatherApp.Services.Weather;
using WeatherApp.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class WeatherPageViewModel : BaseViewModel
    {
        private readonly IWeatherService _weatherService;
        private readonly IEventAggregator _eventAggregator;

        private double _currentLat;
        private double _currentLon;

        public string CurrentCity { get; set; }
        public string CurrentCountry { get; set; }
        public CurrentWeatherModel CurrentWeather { get; set; }
        public WeatherDetailsModel SelectedHour { get; set; }
        public bool IsRefreshing { get; set; }
        public Command RefreshCommand { get; set; }
        public Command TryAgainCommand { get; set; }
        public Command YourLocationsCommand { get; set; }
        public Command SettingsCommand { get; set; }
        public Command MenuCommand { get; set; }

        public WeatherPageViewModel(
            INavigationService navigationService,
            IWeatherService weatherService,
            IEventAggregator eventAggregator) : base(navigationService)
        {
            _weatherService = weatherService;
            _eventAggregator = eventAggregator;

            RefreshCommand = new Command(RefreshCommandHandler);
            TryAgainCommand = new Command(TryAgainCommandHandler);
            YourLocationsCommand = new Command(YourLocationsCommandHandler);
            SettingsCommand = new Command(SettingsCommandHandler);
            MenuCommand = new Command(MenuCommandHandler);

            MainState = LayoutState.Loading;
        }

        private async void RefreshCommandHandler()
        {
            if (HasNoInternetConnection)
            {
                IsRefreshing = false;
                return;
            }
            IsRefreshing = true;
            await GetCurrentWeather();
            IsRefreshing = false;
        }

        private async void TryAgainCommandHandler()
        {
            if (HasNoInternetConnection)
                return;
            MainState = LayoutState.Loading;
            await GetCurrentWeather();
        }

        private async void YourLocationsCommandHandler()
        {
            _eventAggregator.GetEvent<MenuEvent>().Publish();
            await _navigationService.NavigateAsync(nameof(YourLocationsPage));
        }

        private async void SettingsCommandHandler()
        {
            _eventAggregator.GetEvent<MenuEvent>().Publish();
            await _navigationService.NavigateAsync(nameof(SettingsPage));
        }

        private void MenuCommandHandler()
        {
            _eventAggregator.GetEvent<MenuEvent>().Publish();
        }

        private void OnSelectedHourChanged()
        {
            CurrentWeather.hourlyWeatherForecast.ToList().ForEach(a => a.isActive = false);
            SelectedHour.isActive = true;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await GetSelectedPlacemarkAndLocation();
            await GetCurrentWeather();
        }

        private async Task GetSelectedPlacemarkAndLocation()
        {
            try
            {
                var listLocJson = await SecureStorage.GetAsync("locations");
                var listLoc = JsonConvert.DeserializeObject<List<LocationModel>>(listLocJson);
                var selectedLocation = listLoc.FirstOrDefault<LocationModel>(l => l.Selected);

                if (selectedLocation != null)
                {
                    CurrentCity = selectedLocation.Locality;
                    CurrentCountry = selectedLocation.CountryName;
                    _currentLat = selectedLocation.Latitude;
                    _currentLon = selectedLocation.Longitude;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task GetCurrentWeather()
        {
            try
            {
                var units = Preferences.Get("units", "metric");
                CurrentWeather = await _weatherService.GetCurrentWeatherAndHourlyForecastByLatLon(_currentLat, _currentLon, units);
                if(CurrentWeather != null)
                {
                    SelectedHour = CurrentWeather.hourlyWeatherForecast[0];
                    SelectedHour.isActive = true;
                    MainState = LayoutState.None;
                }
                else
                {
                    MainState = LayoutState.Error;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MainState = LayoutState.Error;
            }
        }
    }
}
