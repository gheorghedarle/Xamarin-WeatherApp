using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using WeatherApp.Models;
using WeatherApp.Services.Location;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.ViewModels.Dialogs
{
    public class AddLocationDialogViewModel : BindableBase, IDialogAware
    {
        private List<LocationModel> _locations;
        private readonly LocationService _locationService;

        public Command UseCurrentLocationCommand { get; set; }

        public AddLocationDialogViewModel(LocationService locationService)
        {
            _locationService = locationService;

            UseCurrentLocationCommand = new Command<string>(UseCurrentLocationCommandHandler);
        }

        private async void UseCurrentLocationCommandHandler(string locationName)
        {
            Location location = await _locationService.GetLocation(locationName);
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
            RequestClose(null);
        }

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {

        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            var listLocJson = await SecureStorage.GetAsync("locations");
            _locations = JsonConvert.DeserializeObject<List<LocationModel>>(listLocJson);
        }
    }
}
