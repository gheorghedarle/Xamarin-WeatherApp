using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services.LocalSettings;
using WeatherApp.Services.Location;
using WeatherApp.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;

namespace WeatherApp.ViewModels
{
    public class WelcomePageViewModel: BaseViewModel
    {
        private ILocationService _locationService;
        private ILocalSettingsService _localSettingsService;

        public string CurrentLocation { get; set; }

        public DelegateCommand UseCurrentLocationCommand { get; set; }

        public WelcomePageViewModel(
            INavigationService navigationService, 
            ILocationService locationService,
            ILocalSettingsService localSettingsService) : base(navigationService) 
        {
            _locationService = locationService;
            _localSettingsService = localSettingsService;

            UseCurrentLocationCommand = new DelegateCommand(UseCurrentLocationCommandHandler);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadAndSaveLocalSettings();
        }

        private async void UseCurrentLocationCommandHandler()
        {
            if (HasNoInternetConnection)
                return;
            MainState = LayoutState.Loading;
            try
            {
                var location = await _locationService.GetCurrentLocationCoordinates();
                if (location != null)
                {
                    var placemark = await _locationService.GetCurrentLocationName(location.Latitude, location.Longitude);
                    await SaveLocationAndPlacemark(location, placemark);
                    await _navigationService.NavigateAsync(nameof(WeatherPage));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                MainState = LayoutState.None;
            }
        }

        private async Task SaveLocationAndPlacemark(Location location, Placemark placemark)
        {
            var loc = new LocationModel()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Locality = placemark.Locality,
                CountryName = placemark.CountryName,
                Selected = true
            };
            var listLoc = new List<LocationModel>();
            listLoc.Add(loc);
            await SecureStorage.SetAsync("locations", JsonConvert.SerializeObject(listLoc));
        }

        private async Task LoadAndSaveLocalSettings()
        {
            var localSettings = _localSettingsService.LoadLocalSettings();
            await SecureStorage.SetAsync("weatherApiBaseUrl", localSettings.WeatherApiBaseUrl);
            await SecureStorage.SetAsync("weatherApiKey", localSettings.WeatherApiKey);
        }
    }
}
