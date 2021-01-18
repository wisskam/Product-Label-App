using WPF_ProductLabel.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WPF_ProductLabel.Tools;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using WPF_ProductLabel.StaticResources;

namespace WPF_ProductLabel.ViewModel
{
    class DataLoaderViewModel : BindableBase
    {
        public RelayCommand OpenFileCommand { get; set; }

        private string _dataFilePath;
        private DataTable _loadedData;
        private bool _loading;
        private Layout _selectedLayout;
        private LayoutFieldMap _layoutFieldMap;

        public DataTable LoadedData
        {
            get => _loadedData;
            set 
            { 
                _loadedData = value;
                OnPropertyChanged("LoadedData");
            }
        }

        public bool Loading
        {
            get => _loading;
            set
            {
                _loading = value;
                OnPropertyChanged("LoadingVisibility");
            }
        }
        public string LoadingVisibility
        {
            get
            {
                if (Loading)
                {
                    return "Visible";
                }
                return "Hidden";
            }
        }
        public Layout SelectedLayout
        {
            get => _selectedLayout;
            set
            {
                _selectedLayout = value;
                OnPropertyChanged("SelectedLayout");
            }
        }

        public LayoutFieldMap LayoutFieldMap
        {
            get => _layoutFieldMap;
            set
            {
                _layoutFieldMap = value;
                OnPropertyChanged("LayoutFieldMap");
            }
        }

        public DataLoaderViewModel(Layout selectedLayout)
        {
            OpenFileCommand = new RelayCommand(
                async o => await OnOpenFileAsync(), o => CanOpenFile());
            SelectedLayout = selectedLayout;
        }

        public void enableLoader()
        {
            Loading = true;
        }
        
        public void disableLoader()
        {
            Loading = false;
        }

        private bool CanOpenFile()
        {
            return true;
        }

        private async Task OnOpenFileAsync()
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".xlsx"; // Default file extension
            dlg.Filter = "Excel Spreadsheet (.xlsx)|*.xlsx"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                _dataFilePath = dlg.FileName;

                ExcelFileHandler excelFileHandler = new ExcelFileHandler();
                LoadedData = await excelFileHandler.LoadAsync(_dataFilePath);

                foreach (DataColumn column in LoadedData.Columns)
                {
                    LayoutFieldMap.AddColumn(column);
                }
            }
        }

    }
}
