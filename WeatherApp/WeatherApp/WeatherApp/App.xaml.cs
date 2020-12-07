using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using WeatherApp.Core;
using WeatherApp.Services.LocalSettings;
using WeatherApp.Services.Location;
using WeatherApp.ViewModels;
using WeatherApp.Views;
using Xamarin.Forms;

[assembly: ExportFont("FrankRuhlLibre-Light.ttf", Alias = "FrankRuhle_Light")]
[assembly: ExportFont("FrankRuhlLibre-Regular.ttf", Alias = "FrankRuhle_Regular")]
[assembly: ExportFont("FrankRuhlLibre-Medium.ttf", Alias = "FrankRuhle_Medium")]
[assembly: ExportFont("FrankRuhlLibre-Bold.ttf", Alias = "FrankRuhle_Bold")]
[assembly: ExportFont("FrankRuhlLibre-Black.ttf", Alias = "FrankRuhle_Black")]

[assembly: ExportFont("Rubik-Light.ttf", Alias = "Rubik_Light")]
[assembly: ExportFont("Rubik-Regular.ttf", Alias = "Rubik_Regular")]
[assembly: ExportFont("Rubik-Medium.ttf", Alias = "Rubik_Medium")]
[assembly: ExportFont("Rubik-SemiBold.ttf", Alias = "Rubik_SemiBold")]
[assembly: ExportFont("Rubik-Bold.ttf", Alias = "Rubik_Bold")]

namespace WeatherApp
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        public new static App Current => Application.Current as App;

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(EntryPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(new HttpClientFactory());
            containerRegistry.Register<ILocationService, LocationService>();
            containerRegistry.Register<ILocalSettingsService, LocalSettingsService>();

            containerRegistry.RegisterForNavigation<NavigationPage>("NavigationPage");
            containerRegistry.RegisterForNavigation<EntryPage, EntryPageViewModel>("EntryPage");
            containerRegistry.RegisterForNavigation<WeatherPage, WeatherPageViewModel>("WeatherPage");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
