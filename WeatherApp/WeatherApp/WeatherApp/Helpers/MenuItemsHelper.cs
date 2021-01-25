using System.Collections.ObjectModel;
using WeatherApp.Models;

namespace WeatherApp.Helpers
{
    public static class MenuItemsHelper
    {
        public static ObservableCollection<MenuItemModel> Items = new ObservableCollection<MenuItemModel>() {
            new MenuItemModel()
            {
                Icon = "\uf3c5",
                Label = "Locations"
            },
            new MenuItemModel()
            {
                Icon = "\uf013",
                Label = "Settings"
            }
        };
    }
}
