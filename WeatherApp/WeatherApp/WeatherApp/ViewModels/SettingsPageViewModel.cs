using Prism.Navigation;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class SettingsPageViewModel: BaseViewModel
    {
        public bool IsDarkMode { get; set; }
        public int Units { get; set; }
        public Command BackCommand { get; set; }
        public Command DarkModeToggleCommand { get; set; }

        public SettingsPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            BackCommand = new Command(BackCommandHandler);
            DarkModeToggleCommand = new Command(DarkModeToggleCommandHandler);

            MainState = LayoutState.Loading;
        }
        private async void BackCommandHandler()
        {
            await _navigationService.GoBackAsync();
        }

        private void DarkModeToggleCommandHandler()
        {
            if(IsDarkMode)
            {
                Application.Current.UserAppTheme = OSAppTheme.Dark;
                Preferences.Set("theme", "dark");
            }
            else
            {
                Application.Current.UserAppTheme = OSAppTheme.Light;
                Preferences.Set("theme", "light");
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            IsDarkMode = Application.Current.UserAppTheme.Equals(OSAppTheme.Dark);
            Units = Preferences.Get("units", 0);

            MainState = LayoutState.None;
        }
    }
}
