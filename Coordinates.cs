using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LECO
{
    public record Coordinates
    {
        public Coordinates(double longitude, double latitude)
        {
            if(longitude < 0 || latitude < 0)
            {
                throw new ArgumentException($"{nameof(longitude)}: {longitude} and {nameof(latitude)} : {latitude} cannot be a negative number,");
            }
            Longitude = longitude;
            Latitude = latitude;
        }

        public double Longitude { get; }
        public double Latitude { get; }
    }
}
