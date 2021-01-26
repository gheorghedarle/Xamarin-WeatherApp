using WeatherApp.Models;
using WeatherApp.Views.Templates;
using Xamarin.Forms;

namespace WeatherApp.Helpers.TemplateSelector
{
    public class LocationTemplateSelector: DataTemplateSelector
    {
        public DataTemplate LocationSelectedItemTemplate { get; set; }
        public DataTemplate LocationItemTemplate { get; set; }
        public DataTemplate LocationAddTemplate { get; set; }

        public LocationTemplateSelector()
        {
            LocationSelectedItemTemplate = new DataTemplate(typeof(LocationSelectedItemTemplate));
            LocationItemTemplate = new DataTemplate(typeof(LocationItemTemplate));
            LocationAddTemplate = new DataTemplate(typeof(LocationAddTemplate));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item.GetType() == typeof(LocationModel))
            {
                var locationItem = item as LocationModel;
                if(string.IsNullOrEmpty(locationItem.CountryName) && string.IsNullOrEmpty(locationItem.Locality))
                {
                    return LocationAddTemplate;
                }
                else if(locationItem.Selected)
                {
                    return LocationSelectedItemTemplate;
                }
                else
                {
                    return LocationItemTemplate;
                }
            }
            return LocationItemTemplate;
        }
    }
}
