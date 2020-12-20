using Prism.Events;
using Prism.Navigation;
using System;
using WeatherApp.Events;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherPage : ContentPage, IDestructible
    {
        private OpenMenuEvent _openMenuEvent;
        private CloseMenuEvent _closeMenuEvent;

        public WeatherPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            _openMenuEvent = eventAggregator.GetEvent<OpenMenuEvent>();
            _openMenuEvent.Subscribe(OpenAnimation);

            _closeMenuEvent = eventAggregator.GetEvent<CloseMenuEvent>();
            _closeMenuEvent.Subscribe(CloseAnimation);
        }

        public void Destroy()
        {
            _openMenuEvent.Unsubscribe(OpenAnimation);
            _closeMenuEvent.Unsubscribe(CloseAnimation);
        }

        private async void OpenAnimation()
        {
            menuView.Open(OpenSwipeItem.LeftItems);
        }

        private async void CloseAnimation()
        {
            menuView.Close();
        }
    }
}