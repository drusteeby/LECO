﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LECO.ViewModels
{
    public class CanvasItemViewModelBase : BindableBase
    {
        private double _left;
        public double Left
        {
            get => _left;
            set => SetProperty(ref _left, value);
        }

        private double _top;
        public double Top
        {
            get => _top;
            set => SetProperty(ref _top, value);
        }
    }
}
