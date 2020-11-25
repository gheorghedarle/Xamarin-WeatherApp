using Prism.Navigation;
using System;
using System.Diagnostics;
using WeatherApp.Services.Location;

namespace WeatherApp.ViewModels
{
    public class EntryPageViewModel: BaseViewModel
    {
        private ILocationService _locationService;

        public string CurrentLocation { get; set; }

        public EntryPageViewModel(
            INavigationService navigationService, 
            ILocationService locationService): base(navigationService) 
        {
            _locationService = locationService;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            try
            {
                var location = await _locationService.GetCurrentLocationCoordinates();
                if (location != null)
                {
                    //CurrentLocation = String.Format("Lat: {0}, Long: {1}", location.Latitude, location.Longitude);
                    var placemark = await _locationService.GetCurrentLocationName(location.Latitude, location.Longitude);
                    CurrentLocation = String.Format("{0}, {1}", placemark.Locality, placemark.CountryName);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            IsBusy = false;
        }
    }
}
