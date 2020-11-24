using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace WeatherApp.Services.Location
{
    public class LocationService : ILocationService
    {
        private CancellationTokenSource _cts;

        public async Task<Xamarin.Essentials.Location> GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                _cts = new CancellationTokenSource();
                Xamarin.Essentials.Location location = await Geolocation.GetLocationAsync(request, _cts.Token);

                return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                return null;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                return null;
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                return null;
            }
            catch (Exception ex)
            {
                // Unable to get location
                return null;
            }
        }
    }
}
