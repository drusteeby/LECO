using LECO.Models;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace LECO.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        private City _selectedCity;
        public City SelectedCity
        {
            get => _selectedCity;
            set => SetProperty(ref _selectedCity, value);
        }

        private City _homeCity;
        public City HomeCity
        {
            get => _homeCity;
            set => SetProperty(ref _homeCity, value);
        }

        private bool _inProgress;
        public bool InProgress
        {
            get => _inProgress;
            set => SetProperty(ref _inProgress, value);
        }

        private CalculatedPermutation _results;
        public CalculatedPermutation Results
        {
            get => _results;
            set => SetProperty(ref _results, value);
        }

        public ObservableCollection<City> LoadedCities { get; set; }
        public ObservableCollection<City> SelectedCities { get; set; }

        public ObservableCollection<CanvasItemViewModelBase> MapItems { get; set; }
        public MainWindowViewModel()
        {
            _fileName = String.Empty;
            LoadedCities = new ObservableCollection<City>();
            SelectedCities = new ObservableCollection<City>();
            MapItems = new ObservableCollection<CanvasItemViewModelBase>();

            SelectedCities.CollectionChanged += SelectedCities_CollectionChanged;
        }
        private void SelectedCities_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            MapItems.Clear();
            foreach (var city in SelectedCities)
            {
                MapItems.Add(new CityViewModel(city));
            }
            if(HomeCity != null)
            {
                MapItems.Add(new CityViewModel(HomeCity));
            }    
        }

        private DelegateCommand _selectFile;
        public DelegateCommand SelectFile =>
            _selectFile ?? (_selectFile = new DelegateCommand(ExecuteSelectFile, canExecuteMethod: ()=> !InProgress).ObservesProperty(() =>InProgress));

        private void ExecuteSelectFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt", //limit to text files
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) //set initial directory to My Documents
            };

            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            LoadedCities.Clear();
            FileName = openFileDialog.FileName;
            var allLines = File.ReadAllLines(FileName);

            foreach (var line in allLines)
            {
                var split = line.Split(',');
                if (split.Length != 3)
                {
                    continue;
                }
                var cityName = split[0];
                var latParseSuccessful = double.TryParse(split[1], out var latitude);
                var longParseSuccessful = double.TryParse(split[2], out var longditude);

                if (latParseSuccessful && longParseSuccessful)
                {
                    LoadedCities.Add(new City(cityName, longditude, latitude));
                }
            }
        }

        private bool CanEditParameters() => SelectedCity is not null && !InProgress;

        private DelegateCommand _addSelectedToDestinationCommand;
        public DelegateCommand AddSelectedToDestinationCommand =>
            _addSelectedToDestinationCommand ??= new DelegateCommand(ExecuteAddSelectedToDestinationCommand, 
                                                                     canExecuteMethod: CanEditParameters)
                                                                    .ObservesProperty(() => SelectedCity)
                                                                    .ObservesProperty(() => InProgress);

        private DelegateCommand<City> _removeSelectionCommand;
        public DelegateCommand<City> RemoveSelectionCommand =>
            _removeSelectionCommand ?? (_removeSelectionCommand = new DelegateCommand<City>(ExecuteRemoveSelectionCommand));

        public void ExecuteRemoveSelectionCommand(City toRemove) => SelectedCities.Remove(toRemove);

        private void ExecuteAddSelectedToDestinationCommand()
        {
            if (SelectedCities.Contains(SelectedCity) || SelectedCity == HomeCity)
            {
                return;
            }

            SelectedCities.Add(SelectedCity);
            StartCalculationCommand.RaiseCanExecuteChanged();
        }

        private DelegateCommand _setHomeCityCommand;
        public DelegateCommand SetHomeCityCommand => 
            _setHomeCityCommand ??= new DelegateCommand(ExecuteSetHomeCityCommand,
                                                        canExecuteMethod: CanEditParameters)
                                                        .ObservesProperty(() => SelectedCity)
                                                        .ObservesProperty(() => InProgress);

        void ExecuteSetHomeCityCommand()
        {
            var oldHomeCity = MapItems.SingleOrDefault(vm => vm is CityViewModel cityVm && cityVm.City == HomeCity);
            if (oldHomeCity != null)
            {
                MapItems.Remove(oldHomeCity);
            }

            HomeCity = SelectedCity;
            MapItems.Add(new CityViewModel(HomeCity));
        }
        private DelegateCommand _startCalculationCommand;
        public DelegateCommand StartCalculationCommand => 
            _startCalculationCommand ??= new DelegateCommand(ExecuteStartCalculationCommand, 
                                                             CanStartCommand)
                                                            .ObservesProperty(() => InProgress);

        private bool CanStartCommand() => (SelectedCities.Count > 2 && !InProgress);

        async void ExecuteStartCalculationCommand()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                InProgress = true;
                var calculator = new CityDistanceCalculator();
                var result = await calculator.GetShortestDistanceAsync(HomeCity, SelectedCities, _cancellationTokenSource.Token);

                for (var cityNum = 0; cityNum < result.TravelItinerary.Count - 1; cityNum++)
                {
                    MapItems.Add(new LineViewModel(result.TravelItinerary[cityNum], result.TravelItinerary[cityNum + 1]));
                }

                Results = result;
            }
            catch (Exception ex)
            {
                //add error message or popup saying unsuccessful
            }
            finally
            {
                InProgress = false;
            }
        }

        private CancellationTokenSource _cancellationTokenSource;
        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(ExecuteCancelCommand, () => InProgress)
                                                                   .ObservesProperty(() => InProgress));
        void ExecuteCancelCommand()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
