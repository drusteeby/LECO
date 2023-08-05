using LECO.Models;
using LECO.ViewModels;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
