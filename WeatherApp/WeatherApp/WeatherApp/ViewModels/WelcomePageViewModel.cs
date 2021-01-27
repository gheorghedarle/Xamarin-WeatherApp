using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services.LocalSettings;
using WeatherApp.Services.Location;
using WeatherApp.Views;
using WeatherApp.Views.Dialogs;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class WelcomePageViewModel: BaseViewModel
    {
        #region Private & Protected

        private ILocationService _locationService;
        private ILocalSettingsService _localSettingsService;
        private IDialogService _dialogService;

        #endregion

        #region Properties

        public string CurrentLocation { get; set; }

        #endregion

        #region Commands

        public Command UseCurrentLocationCommand { get; set; }
        public Command AddLocationCommand { get; set; }

        #endregion

        #region Constructors

        public WelcomePageViewModel(
            INavigationService navigationService, 
            ILocationService locationService,
            ILocalSettingsService localSettingsService,
            IDialogService dialogService) : base(navigationService) 
        {
            _locationService = locationService;
            _localSettingsService = localSettingsService;
            _dialogService = dialogService;

            UseCurrentLocationCommand = new Command(UseCurrentLocationCommandHandler);
            AddLocationCommand = new Command(AddLocationCommandHandler);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadAndSaveLocalSettings();
        }

        #endregion

        #region Command Handlers

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

        private async void AddLocationCommandHandler()
        {
            var param = new DialogParameters()
            {
                { "fromPage" , "welcome" }
            };
            await _dialogService.ShowDialogAsync(nameof(AddLocationDialog), param);
        }

        #endregion

        #region Private Methods

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

        #endregion
    }
}
