using System.Threading.Tasks;

namespace WeatherApp.Services.Location
{
    public interface ILocationService
    {
        Task<Xamarin.Essentials.Location> GetCurrentLocation();
    }
}
