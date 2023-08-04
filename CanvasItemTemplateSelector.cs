using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using LECO.ViewModels;

namespace LECO
{
    public class CanvasItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CityTemplate { get; set; }

        public DataTemplate LineTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is LineViewModel)
            {
                return LineTemplate;
            }
            if (item is CityViewModel)
            {
                return CityTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
