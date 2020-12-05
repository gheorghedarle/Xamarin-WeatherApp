using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.ViewModels
{
    public class WeatherPageViewModel : BaseViewModel
    {
        public string CurrentDate { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentCountry { get; set; }

        public WeatherPageViewModel(INavigationService navigationService): base(navigationService)
        {
            CurrentCity = "Timișoara";
            CurrentCountry = "Romania";
            CurrentDate = CreateDateString();
        }

        private string CreateDateString()
        {
            var currentDate = DateTime.Now;
            return currentDate.ToString("dddd, d MMMM yyyy");
        }
    }
}
