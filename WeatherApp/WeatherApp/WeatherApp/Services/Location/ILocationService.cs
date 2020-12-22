using System.Threading.Tasks;

namespace WeatherApp.Services.Location
{
    public interface ILocationService
    {
        Task<Xamarin.Essentials.Location> GetCurrentLocationCoordinates();
        Task<Xamarin.Essentials.Placemark> GetCurrentLocationName(double lat, double lon);
        Task<Xamarin.Essentials.Location> GetLocation(string locationName);

    }
}
