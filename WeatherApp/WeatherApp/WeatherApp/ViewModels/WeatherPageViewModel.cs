using Newtonsoft.Json;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Events;
using WeatherApp.Helpers;
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
        #region Private & Protected

        private readonly IWeatherService _weatherService;
        private readonly IEventAggregator _eventAggregator;

        private double _currentLat;
        private double _currentLon;

        #endregion

        #region Properties

        public string CurrentCity { get; set; }
        public string CurrentCountry { get; set; }
        public double CurrentTemp { get; set; }
        public CurrentWeatherModel CurrentWeather { get; set; }
        public WeatherDetailsModel SelectedHour { get; set; }
        public bool IsRefreshing { get; set; }
        public ObservableCollection<MenuItemModel> MenuItems { get; set; }

        #endregion

        #region Commands

        public Command RefreshCommand { get; set; }
        public Command TryAgainCommand { get; set; }
        public Command SideMenuCommand { get; set; }
        public Command MenuCommand { get; set; }

        #endregion

        #region Constructors

        public WeatherPageViewModel(
            INavigationService navigationService,
            IWeatherService weatherService,
            IEventAggregator eventAggregator) : base(navigationService)
        {
            _weatherService = weatherService;
            _eventAggregator = eventAggregator;

            RefreshCommand = new Command(RefreshCommandHandler);
            TryAgainCommand = new Command(TryAgainCommandHandler);
            SideMenuCommand = new Command<string>(SideMenuCommandHandler);
            MenuCommand = new Command(MenuCommandHandler);

            MenuItems = MenuItemsHelper.Items;
        }

        #endregion

        #region Command Handlers

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

        private async void SideMenuCommandHandler(string label)
        {
            _eventAggregator.GetEvent<MenuEvent>().Publish();
            switch(label)
            {
                case "Locations": await _navigationService.NavigateAsync(nameof(YourLocationsPage)); break;
                case "Settings": await _navigationService.NavigateAsync(nameof(SettingsPage)); break;
            }
        }

        private void MenuCommandHandler()
        {
            _eventAggregator.GetEvent<MenuEvent>().Publish();
        }

        #endregion

        #region Properties Changed Event Handlers

        private void OnSelectedHourChanged()
        {
            CurrentWeather.hourlyWeatherForecast.ToList().ForEach(a => a.isActive = false);
            AnimateTemp(SelectedHour.temp);
            SelectedHour.isActive = true;
        }

        #endregion

        #region Navigation

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await GetSelectedPlacemarkAndLocation();
            await GetCurrentWeather();
        }

        #endregion

        #region Private Methods

        private void AnimateTemp(double temp)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var sign = temp > CurrentTemp ? 1 : -1;

            Device.StartTimer(TimeSpan.FromSeconds(1 / 1000f), () =>
            {
                double t = stopwatch.Elapsed.TotalMilliseconds % 500 / 1000;
                if(sign == 1)
                {
                    CurrentTemp = Math.Min((double)temp, (double)t + CurrentTemp);
                    if (CurrentTemp >= (double)temp)
                    {
                        stopwatch.Stop();
                        return false;
                    }
                }
                else
                {
                    CurrentTemp = Math.Max((double)temp, CurrentTemp - (double)t);
                    if (CurrentTemp <= (double)temp)
                    {
                        stopwatch.Stop();
                        return false;
                    }
                }


                return true;
            });
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

        #endregion
    }
}
