using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherApp.Models;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class WeatherDetailsPageViewModel : BaseViewModel
    {
        public string CurrentCity { get; set; }
        public string CurrentCountry { get; set; }
        public WeatherDetailsModel CurrentDayWeather { get; set; }
        public Command BackCommand { get; set; }

        public WeatherDetailsPageViewModel(
            INavigationService navigationService): base(navigationService)
        {
            BackCommand = new Command(BackCommandHandler);

            MainState = LayoutState.Loading;
        }

        private async void BackCommandHandler()
        {
            await _navigationService.GoBackAsync();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            CurrentCity = parameters.GetValue<string>("currentCity");
            CurrentCountry = parameters.GetValue<string>("currentCountry");
            CurrentDayWeather = parameters.GetValue<WeatherDetailsModel>("currentDayWeather");

            MainState = LayoutState.None;
        }
    }
}
