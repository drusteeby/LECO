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
    public class LineViewModel: CanvasItemViewModelBase
    {
        public LineViewModel(City start, City end)
        {
            if (start is null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            if (end is null)
            {
                throw new ArgumentNullException(nameof(end));
            }

            X1 = start.Point.X;
            Y1 = start.Point.Y;
            X2 = end.Point.X;
            Y2 = end.Point.Y;
        }

        private double _x1;
        public double X1
        {
            get => _x1;
            set => SetProperty(ref _x1, value);
        }

        private double _x2;
        public double X2
        {
            get => _x2;
            set => SetProperty(ref _x2, value);
        }

        private double _y1;
        public double Y1
        {
            get => _y1;
            set => SetProperty(ref _y1, value);
        }

        private double _y2;
        public double Y2
        {
            get => _y2;
            set => SetProperty(ref _y2, value);
        }
    }
}
