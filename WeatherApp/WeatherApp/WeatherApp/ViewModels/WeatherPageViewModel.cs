using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.ViewModels
{
    public class WeatherPageViewModel : BaseViewModel
    {
        public WeatherPageViewModel(INavigationService navigationService): base(navigationService)
        { }
    }
}
