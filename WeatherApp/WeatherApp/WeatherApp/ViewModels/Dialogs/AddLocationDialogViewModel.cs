using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using WeatherApp.Models;
using WeatherApp.Services.Location;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.ViewModels.Dialogs
{
    public class AddLocationDialogViewModel : BaseViewModel, IDialogAware
    {
        private List<LocationModel> _locations;
        private readonly LocationService _locationService;

        public Command UseCurrentLocationCommand { get; set; }
        public bool HasError { get; set; }

        public AddLocationDialogViewModel(
            INavigationService navigationService,
            LocationService locationService): base(navigationService)
        {
            _locationService = locationService;

            UseCurrentLocationCommand = new Command<string>(UseCurrentLocationCommandHandler);

            MainState = LayoutState.Loading;
        }

        private async void UseCurrentLocationCommandHandler(string locationName)
        {
            MainState = LayoutState.Loading;
            HasError = false;
            Location location = await _locationService.GetLocation(locationName);
            if(location == null)
            {
                HasError = true;
                MainState = LayoutState.None;
                return;
            }
            Placemark placemark = await _locationService.GetCurrentLocationName(location.Latitude, location.Longitude);
            var loc = new LocationModel()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Locality = placemark.Locality,
                CountryName = placemark.CountryName,
                Selected = false
            };
            _locations.Add(loc);
            await SecureStorage.SetAsync("locations", JsonConvert.SerializeObject(_locations));
            MainState = LayoutState.None;
            RequestClose(null);
        }

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {}

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            var listLocJson = await SecureStorage.GetAsync("locations");
            _locations = JsonConvert.DeserializeObject<List<LocationModel>>(listLocJson);
            MainState = LayoutState.None;
        }
    }
}
