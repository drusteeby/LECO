using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LECO
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            _fileName = String.Empty;
            LoadedCities = new ObservableCollection<City>();
            SelectFile = new DelegateCommand(OnSelectFileCommand);
        }

        private void OnSelectFileCommand()
        {
            LoadedCities.Clear();

            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt", //limit to text files
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) //set initial directory to My Documents
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
            }

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

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }
        public DelegateCommand SelectFile { get; }

        public ObservableCollection<City> LoadedCities { get; set; }
        public ObservableCollection<City> SelectedCities { get; set; }

        private City _selectedCity;
        public City SelectedCity 
        {
            get => _selectedCity;
            set => SetProperty(ref _selectedCity, value);
        }
    }
}
