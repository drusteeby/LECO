using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LECO
{
    public record CalculatedPermutation : IComparable<CalculatedPermutation>
    {
        public CalculatedPermutation(IList<City> travelItinerary, double totalDistance)
        {
            if(totalDistance < 0)
            {
                throw new ArgumentException($"Parameter {nameof(totalDistance)} : {totalDistance} cannot be a negative number.");
            }

            TotalDistance = totalDistance;
            TravelItinerary = travelItinerary ?? throw new ArgumentNullException(nameof(travelItinerary));
        }
        public double TotalDistance { get; }
        public IList<City> TravelItinerary { get; }

        public int CompareTo(CalculatedPermutation? other)
        {
            if (other == null)
            {
                return 0;
            }

            if (TotalDistance < other.TotalDistance)
            {
                return -1;
            }
            if(TotalDistance > other.TotalDistance)
            { 
                return 1; 
            }

            return 0;
        }
    }
}
