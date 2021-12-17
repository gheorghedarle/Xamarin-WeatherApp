using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationItemTemplate : Frame
    {
        public LocationItemTemplate()
        {
            InitializeComponent();
        }
    }
}