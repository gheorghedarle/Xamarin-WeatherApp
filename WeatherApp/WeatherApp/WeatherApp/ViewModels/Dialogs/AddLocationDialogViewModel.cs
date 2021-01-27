using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherApp.Models;
using WeatherApp.Services.Location;
using WeatherApp.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.ViewModels.Dialogs
{
    public class AddLocationDialogViewModel : BaseViewModel, IDialogAware
    {
        #region Private & Protected

        private readonly LocationService _locationService;

        private List<LocationModel> _locations;
        private string _fromPage;

        #endregion

        #region Properties

        public Command UseCurrentLocationCommand { get; set; }
        public bool HasError { get; set; }

        #endregion

        #region Constructors

        public AddLocationDialogViewModel(
            INavigationService navigationService,
            LocationService locationService): base(navigationService)
        {
            _locationService = locationService;

            UseCurrentLocationCommand = new Command<string>(UseCurrentLocationCommandHandler);

            MainState = LayoutState.Loading;
        }

        #endregion

        #region Command Handlers

        private async void UseCurrentLocationCommandHandler(string locationName)
        {
            MainState = LayoutState.Loading;
            HasError = false;
            Location location = await _locationService.GetLocation(locationName);
            if (location == null)
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
            _locations.ForEach(l => l.Selected = false);
            _locations.First(l => l.Locality == loc.Locality).Selected = true;
            await SecureStorage.SetAsync("locations", JsonConvert.SerializeObject(_locations));
            MainState = LayoutState.None;

            RequestClose(null);
            if (_fromPage == "welcome")
            {
                await _navigationService.NavigateAsync(nameof(WeatherPage));
            }
        }

        #endregion

        #region Dialog

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {}

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            _fromPage = parameters.GetValue<string>("fromPage");

            var listLocJson = await SecureStorage.GetAsync("locations");
            if(!string.IsNullOrEmpty(listLocJson))
            {
                _locations = JsonConvert.DeserializeObject<List<LocationModel>>(listLocJson);
            }
            else
            {
                _locations = new List<LocationModel>();
            }

            MainState = LayoutState.None;
        }

        #endregion
    }
}
