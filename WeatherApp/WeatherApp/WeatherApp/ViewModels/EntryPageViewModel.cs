using Prism.Commands;
using Prism.Navigation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WeatherApp.Services.Location;
using WeatherApp.Views;
using Xamarin.Essentials;

namespace WeatherApp.ViewModels
{
    public class EntryPageViewModel: BaseViewModel
    {
        private ILocationService _locationService;

        public string CurrentLocation { get; set; }

        public DelegateCommand UseCurrentLocationCommand { get; set; }

        public EntryPageViewModel(
            INavigationService navigationService, 
            ILocationService locationService): base(navigationService) 
        {
            _locationService = locationService;

            UseCurrentLocationCommand = new DelegateCommand(UseCurrentLocationCommandHandler);
        }

        private async void UseCurrentLocationCommandHandler()
        {
            IsBusy = true;
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
            IsBusy = false;
        }

        private async Task SaveLocationAndPlacemark(Location location, Placemark placemark)
        {
            await SecureStorage.SetAsync("currentLatitude", location.Latitude.ToString());
            await SecureStorage.SetAsync("currentLongitude", location.Longitude.ToString());
            await SecureStorage.SetAsync("currentCity", placemark.Locality);
            await SecureStorage.SetAsync("currentCountry", placemark.CountryName);
        }
    }
}
