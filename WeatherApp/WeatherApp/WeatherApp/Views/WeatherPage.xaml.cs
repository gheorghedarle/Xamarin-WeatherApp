﻿using Prism.Events;
using Prism.Navigation;
using System;
using WeatherApp.Events;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherPage : ContentPage, IDestructible
    {
        private MenuEvent _menuEvent;

        public WeatherPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            _menuEvent = eventAggregator.GetEvent<MenuEvent>();
            _menuEvent.Subscribe(OpenCloseMenu);
        }

        public void Destroy()
        {
            _menuEvent.Unsubscribe(OpenCloseMenu);
        }

        private void OpenCloseMenu()
        {
            menuView.State = SideMenuState.LeftMenuShown;
        }
    }
}