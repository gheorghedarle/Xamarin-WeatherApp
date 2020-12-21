using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
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
        public bool IsMenuOpen { get; set; }
        public bool IsRefreshing { get; set; }
        public Command RefreshCommand { get; set; }
        public Command TryAgainCommand { get; set; }
        public Command YourLocationsCommand { get; set; }
        public Command SettingsCommand { get; set; }
        public Command MenuCommand { get; set; }
        public Command DayForecastCommand { get; set; }
        public double MenuSize { get; set; } = DeviceDisplay.MainDisplayInfo.Width / 4;

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
            DayForecastCommand = new Command<WeatherDetailsModel>(DayForecastCommandHandler);

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
            await _navigationService.NavigateAsync(nameof(YourLocationsPage));
        }

        private async void SettingsCommandHandler()
        {
            await _navigationService.NavigateAsync(nameof(SettingsPage));
        }

        private async void DayForecastCommandHandler(WeatherDetailsModel item)
        {
            await _navigationService.NavigateAsync(nameof(WeatherDetailsPage));
        }

        private void MenuCommandHandler()
        {
            if(IsMenuOpen)
            {
                _eventAggregator.GetEvent<OpenMenuEvent>().Publish();
            }
            else
            {
                _eventAggregator.GetEvent<CloseMenuEvent>().Publish();
            }
            IsMenuOpen = !IsMenuOpen;
        }

        private void OnSelectedHourChanged()
        {
            CurrentWeather.hourlyWeatherForecast.ToList().ForEach(a => a.isActive = false);
            SelectedHour.isActive = true;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await GetCurrentLocalityAndCountry();
            await GetCurrentLatLon();
            await GetCurrentWeather();
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
            try
            {
                CurrentWeather = await _weatherService.GetCurrentWeatherAndHourlyForecastByLatLon(_currentLat, _currentLon);
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
