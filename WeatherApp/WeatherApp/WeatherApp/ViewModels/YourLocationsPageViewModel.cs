using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services.Location;
using WeatherApp.Views.Dialogs;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace WeatherApp.ViewModels
{
    public class YourLocationsPageViewModel: BaseViewModel
    {
        #region Private & Protected

        private readonly ILocationService _locationService;
        private readonly IDialogService _dialogService;

        #endregion

        #region Properties

        public ObservableCollection<LocationModel> Locations { get; set; }

        #endregion

        #region Commands

        public Command BackCommand { get; set; }
        public Command AddLocationCommand { get; set; }
        public Command SelectLocationCommand { get; set; }
        public Command DeleteLocationCommand { get; set; }

        #endregion

        #region Constructors

        public YourLocationsPageViewModel(
            INavigationService navigationService,
            ILocationService locationService,
            IDialogService dialogService):base(navigationService)
        {
            _locationService = locationService;
            _dialogService = dialogService;

            BackCommand = new Command(BackCommandHandler);
            AddLocationCommand = new Command(AddLocationCommandHandler);
            SelectLocationCommand = new Command<string>(SelectLocationCommandHandler);
            DeleteLocationCommand = new Command<string>(DeleteLocationCommandHandler);

            Locations = new ObservableCollection<LocationModel>();
        }

        #endregion

        #region Command Handlers

        private async void BackCommandHandler()
        {
            await _navigationService.GoBackAsync();
        }

        private async void AddLocationCommandHandler()
        {
            var param = new DialogParameters()
            {
                { "fromPage" , "locations" }
            };
            await _dialogService.ShowDialogAsync(nameof(AddLocationDialog), param);
        }

        private async void SelectLocationCommandHandler(string selectedLocality)
        {
            MainState = LayoutState.Loading;

            Locations.ForEach(l => l.Selected = false);
            Locations.First(l => l.Locality == selectedLocality).Selected = true;
            Locations.RemoveAt(Locations.Count - 1);

            await SecureStorage.SetAsync("locations", JsonConvert.SerializeObject(Locations));
            await GetPlacemarkAndLocation();

            MainState = LayoutState.None;
        }

        private async void DeleteLocationCommandHandler(string selectedLocality)
        {
            MainState = LayoutState.Loading;

            if (Locations.Count > 2)
            {
                var item = Locations.First(l => l.Locality == selectedLocality);
                if (item.Selected)
                {
                    var index = Locations.IndexOf(item);
                    if (index < Locations.Count - 2)
                    {
                        Locations[index + 1].Selected = true;
                    }
                    else
                    {
                        Locations[0].Selected = true;
                    }
                }
                Locations.Remove(item);
                Locations.RemoveAt(Locations.Count - 1);

                await SecureStorage.SetAsync("locations", JsonConvert.SerializeObject(Locations));
                await GetPlacemarkAndLocation();
            }

            MainState = LayoutState.None;
        }

        #endregion

        #region Navigation

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            MainState = LayoutState.Loading;

            await GetPlacemarkAndLocation();

            MainState = LayoutState.None;
        }

        #endregion

        #region Private Methods

        private async Task GetPlacemarkAndLocation()
        {
            try
            {
                Locations.Clear();

                var listLocJson = await SecureStorage.GetAsync("locations");
                var locations = JsonConvert.DeserializeObject<List<LocationModel>>(listLocJson);

                locations.ForEach(l => Locations.Add(l));
                Locations.Add(new LocationModel());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #endregion
    }
}
