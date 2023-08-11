using LECO.Models;

namespace LECO.ViewModels
{
    public class CityViewModel : CanvasItemViewModelBase
    {
        public CityViewModel(City city)
        {
            City = city;
            Left = city.Longitude;
            Top = city.Latitude;
        }

        public City City { get; }
    }
}
