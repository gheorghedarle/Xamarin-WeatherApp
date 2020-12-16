using Prism.Commands;
using Prism.Navigation;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services.Weather;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class WeatherPageViewModel : BaseViewModel
    {
        private readonly IWeatherService _weatherService;

        private double _currentLat;
        private double _currentLon;

        public string CurrentCity { get; set; }
        public string CurrentCountry { get; set; }
        public CurrentWeatherModel CurrentWeather { get; set; }
        public bool IsRefreshing { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public Command<HourlyModel> HourlySelectionChangedCommand { get; set; }

        public WeatherPageViewModel(
            INavigationService navigationService,
            IWeatherService weatherService): base(navigationService)
        {
            _weatherService = weatherService;

            RefreshCommand = new DelegateCommand(RefreshCommandHandler);
            HourlySelectionChangedCommand = new Command<HourlyModel>((model) => HourlySelectionChangedCommandHandler(model));

            MainState = LayoutState.Loading;
        }

        private async void RefreshCommandHandler()
        {
            IsRefreshing = true;
            await GetCurrentWeather();
            IsRefreshing = false;
        }

        private void HourlySelectionChangedCommandHandler(HourlyModel item)
        {
            var index = CurrentWeather.hourlyWeatherForecast
                .IndexOf(item);

            if (index > -1)
            {
                CurrentWeather.hourlyWeatherForecast.ToList().ForEach(a => a.isActive = false);

                CurrentWeather.hourlyWeatherForecast[index].isActive = true;
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await GetCurrentLocalityAndCountry();
            await GetCurrentLatLon();
            await GetCurrentWeather();
        }

        private async Task GetCurrentLocalityAndCountry()
        {
            try
            {
                CurrentCity = await SecureStorage.GetAsync("currentCity");
                CurrentCountry = await SecureStorage.GetAsync("currentCountry");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task GetCurrentLatLon()
        {
            try
            {
                var currentLat = await SecureStorage.GetAsync("currentLatitude");
                var currentLon = await SecureStorage.GetAsync("currentLongitude");
                _currentLat = Convert.ToDouble(currentLat);
                _currentLon = Convert.ToDouble(currentLon);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task GetCurrentWeather()
        {
            try
            {
                CurrentWeather = await _weatherService.GetCurrentWeatherAndHourlyForecastByLatLon(_currentLat, _currentLon);
                if(CurrentWeather != null)
                {
                    CurrentWeather.hourlyWeatherForecast[0].isActive = true;
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
    }
}
