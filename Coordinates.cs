using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LECO
{
    public record Coordinates
    {
        public Point Point { get; init; }
        public double Longitude => Point.X;
        public double Latitude => Point.Y;

        public Coordinates(double longitude, double latitude)
        {
            if(longitude < 0 || latitude < 0)
            {
                throw new ArgumentException($"{nameof(longitude)}: {longitude} and {nameof(latitude)} : {latitude} cannot be a negative number,");
            }

            Point = new Point(longitude, latitude);
        }


    }
}
