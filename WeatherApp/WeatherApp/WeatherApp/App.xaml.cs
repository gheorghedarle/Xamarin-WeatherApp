using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using WeatherApp.Core;
using WeatherApp.Services.Location;
using WeatherApp.ViewModels;
using WeatherApp.Views;
using Xamarin.Forms;

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
