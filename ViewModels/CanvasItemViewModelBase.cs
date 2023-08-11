using Prism.Mvvm;

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
