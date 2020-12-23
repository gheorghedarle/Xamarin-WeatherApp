using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Services.Location;
using WeatherApp.Views.Dialogs;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class YourLocationsPageViewModel: BaseViewModel
    {
        private readonly ILocationService _locationService;
        private readonly IDialogService _dialogService;

        public ObservableCollection<LocationModel> Locations { get; set; }

        public Command BackCommand { get; set; }
        public Command AddLocationCommand { get; set; }

        public YourLocationsPageViewModel(
            INavigationService navigationService,
            ILocationService locationService,
            IDialogService dialogService):base(navigationService)
        {
            _locationService = locationService;
            _dialogService = dialogService;

            BackCommand = new Command(BackCommandHandler);
            AddLocationCommand = new Command(AddLocationCommandHandler);

            Locations = new ObservableCollection<LocationModel>();

            MainState = LayoutState.Loading;
        }

        private async void BackCommandHandler()
        {
            await _navigationService.GoBackAsync();
        }

        private async void AddLocationCommandHandler()
        {
            await _dialogService.ShowDialogAsync(nameof(AddLocationDialog));
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await GetPlacemarkAndLocation();

            MainState = LayoutState.None;
        }

        private async Task GetPlacemarkAndLocation()
        {
            try
            {
                Locations.Clear();

                var listLocJson = await SecureStorage.GetAsync("locations");
                var selectedLocationIndexString = await SecureStorage.GetAsync("selectedLocationIndex");

                var listLoc = JsonConvert.DeserializeObject<List<LocationModel>>(listLocJson);
                var selectedLocationIndex = Convert.ToInt32(selectedLocationIndexString);

                listLoc.ForEach(l => Locations.Add(l));
                Locations.Add(new LocationModel());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }

}
