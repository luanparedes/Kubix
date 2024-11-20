using System.Text.Json.Serialization;

namespace Kubix.Model
{
    public class CityModel
    {
        public string City { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string Population { get; set; }

        public float Temperature {  get; set; }
    }
}
