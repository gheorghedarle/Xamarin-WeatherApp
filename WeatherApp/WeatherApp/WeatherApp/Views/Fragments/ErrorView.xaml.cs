using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views.Fragments
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ErrorView : Grid
    {
        public ErrorView()
        {
            InitializeComponent();
        }
    }
}