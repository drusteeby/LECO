using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LECO.Models
{
    public record City : Coordinates
    {
        public City(string name, double longitude, double latitude) : base(longitude, latitude)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
