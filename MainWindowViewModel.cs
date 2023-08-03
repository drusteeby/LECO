using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LECO
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            FileName = String.Empty;
            SelectFile = new DelegateCommand(OnSelectFileCommand);
        }

        private void OnSelectFileCommand()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt", //limit to text files
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) //set initial directory to My Documents
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
            }
        }

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }
        public DelegateCommand SelectFile { get; }
    }
}
