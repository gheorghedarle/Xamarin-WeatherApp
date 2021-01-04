namespace WeatherApp.Models
{
    public class LocationModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Locality { get; set; }
        public string CountryName { get; set; }
        public bool Selected { get; set; }
    }
}
