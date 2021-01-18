using WPF_ProductLabel.Tools;
using WPF_ProductLabel.Model;

using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Data;

namespace WPF_ProductLabel.ViewModel
{
    class LabelGeneratorViewModel : BindableBase
    {
        public RelayCommand GenerateSampleCommand { get; set; }
        public RelayCommand GenerateCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand PreviousLabelCommand { get; set; }
        public RelayCommand NextLabelCommand { get; set; }
        public RelayCommand LoadProductDataCommand { get; set; }
        public RelayCommand SaveProductDataCommand { get; set; }

        private Layout _selectedLayout;
        private Label _selectedLabel;
        private int _selectedLabelNumber = 0;
        public ObservableCollection<Label> _labels;
        private DataLoaderViewModel _dataLoaderViewModel;
        public DataTable _productData;
        public ObservableCollection<Field> _mappedFields;

        public ObservableCollection<Layout> Layouts { get; set; }
        public ObservableCollection<Label> Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                OnPropertyChanged("Labels");
                OnPropertyChanged("LabelsCount");
            }
        }
        public Layout SelectedLayout
        {
            get { return _selectedLayout; }
            set
            {
                _selectedLayout = value;
                OnPropertyChanged("SelectedLayout");
                OnPropertyChanged("IsFormLabelEnabled");
                OnPropertyChanged("IsSelectedLabelVisible");
            }
        }
        public Label SelectedLabel
        {
            get { return _selectedLabel; }
            set
            {
                _selectedLabel = value;
                OnPropertyChanged("SelectedLabel");
                OnPropertyChanged("IsFormLabelEnabled");
                OnPropertyChanged("IsSelectedLabelVisible");
                OnPropertyChanged("CanLayoutChange");
            }
        }
        public bool IsFormLabelEnabled
        {
            get { return SelectedLayout != null && SelectedLabel != null; }
        }
        public string IsSelectedLabelVisible
        {
            get {
                if(SelectedLayout != null && SelectedLabel != null)
                {
                    return "Visible";
                }
                return "Hidden";
            }
        }
        public bool CanLayoutChange
        {
            get {
                return SelectedLabel == null;
            }
        }
        public DataLoaderViewModel DataLoaderViewModel
        {
            get { return _dataLoaderViewModel; }
            set
            {
                _dataLoaderViewModel = value;
                OnPropertyChanged("DataLoaderViewModel");
                OnPropertyChanged("LoadedProductsAmount");
            }
        }
        public DataTable ProductData
        {
            get { return _productData; }
            set
            {
                _productData = value;
                OnPropertyChanged("DataLoaderViewModel");
                OnPropertyChanged("ProductData");
            }
        }

        public LabelGeneratorViewModel()
        {
            AddCommand = new RelayCommand(
                o => OnAddLabel(), o => CanAddLabel());
            DeleteCommand = new RelayCommand(
                o => OnDeleteLabel(), o => CanDeleteLabel());
            PreviousLabelCommand = new RelayCommand(
                o => OnPreviousLabel(), o => CanPreviousLabel());
            NextLabelCommand = new RelayCommand(
                o => OnNextLabel(), o => CanNextLabel());
            GenerateSampleCommand = new RelayCommand(
                o => OnGenerateSample(), o => true);
            GenerateCommand = new RelayCommand(
                o => OnGenerate(), o => CanGenerate());
            LoadProductDataCommand = new RelayCommand(
                o => OnLoadProductData(), o => CanLoadProductData());
            SaveProductDataCommand = new RelayCommand(
                o => OnSaveProductData(), o => CanSaveProductData());
            LoadLayouts();
            Labels = new ObservableCollection<Label>();

            DataLoaderViewModel = new DataLoaderViewModel(SelectedLayout);
        }

        private bool CanLoadProductData()
        {
            return SelectedLayout != null;
                //&& DataLoaderViewModel.SelectedLayout == null;
        }

        private void OnLoadProductData()
        {
            DataLoaderViewModel.SelectedLayout = SelectedLayout;
            if(_mappedFields == null)
            {
                DataLoaderViewModel.LayoutFieldMap = new LayoutFieldMap(SelectedLayout.Fields);
            }
        }

        private bool CanSaveProductData()
        {
            return true;
        }

        private void OnSaveProductData()
        {
            bool canAddToAlreadyLabels = false;
            if(DataLoaderViewModel.LoadedData != null)
            {
                if(_mappedFields != null)
                {
                    if (IsMappingChanged(_mappedFields, DataLoaderViewModel.LayoutFieldMap.Fields))
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show(
                            "Mapping was changed, do you want to regenerate labels(Yes) or add to current set?(No)" +
                            "\nWarning! Regenerating labels will destroy all added labels!",
                            "Field mapping change detected",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question,
                            MessageBoxResult.No
                        );

                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            Labels = new ObservableCollection<Label>();
                            ProductData = null;
                        }
                        else
                        {
                            canAddToAlreadyLabels = true;
                        }
                    }
                }

                if (ProductData != null && !canAddToAlreadyLabels)
                {
                    if (DataLoaderViewModel.LoadedData.Columns.Count
                        == ProductData.Columns.Count
                        && DataLoaderViewModel.LoadedData.Rows.Count
                        == ProductData.Rows.Count)
                    {
                        return;
                    }
                }

                _mappedFields = new ObservableCollection<Field>();
                foreach(Field field in DataLoaderViewModel.LayoutFieldMap.Fields)
                {
                    Field tempMapField = new Field();
                    tempMapField.Set(field);
                    _mappedFields.Add(tempMapField);
                }

                ProductData = DataLoaderViewModel.LoadedData;

                foreach(DataRow row in ProductData.Rows)
                {
                    Label label = new Label();
                    foreach(Field field in _mappedFields)
                    {
                        Field tempField = new Field();
                        tempField.Set(field);

                        if (field.MappedColumn != null)
                        {
                            tempField.Value = row[field.MappedColumn.ColumnName].ToString();
                        }
                        label.Fields.Add(tempField);

                    }
                    AddLabel(label);
                }
            }
        }

        private bool CanDeleteLabel()
        {
            return SelectedLabel != null;
        }

        private void OnDeleteLabel()
        {
            Labels.Remove(SelectedLabel);
            if(Labels.Count > 0 && _selectedLabelNumber > 1)
            {
                OnPreviousLabel();
            }
            else if(Labels.Count > 0 && _selectedLabelNumber == 1)
            {
                _selectedLabelNumber--;
                OnNextLabel();
            }
            else
            {
                SelectedLabel = null;
            }
        }

        private void OnNextLabel()
        {
            _selectedLabelNumber++;
            SelectedLabel = Labels[_selectedLabelNumber - 1];
        }

        private bool CanNextLabel()
        {
            return Labels.Count > 0 
                && _selectedLabelNumber < Labels.Count;
        }

        private void OnPreviousLabel()
        {
            _selectedLabelNumber--;
            SelectedLabel = Labels[_selectedLabelNumber - 1];
        }

        private bool CanPreviousLabel()
        {
            return Labels.Count > 0 && _selectedLabelNumber > 1;
        }

        private bool CanGenerate()
        {
            return Labels.Count > 0 && SelectedLayout != null;
        }

        private void OnGenerate()
        {
            Renderer renderer = new Renderer(SelectedLayout);
            foreach(Label label in Labels)
            {
                renderer.RenderNextLabel2(label);
            }
            renderer.SaveAndOpen();
        }


        private bool CanAddLabel()
        {
            return SelectedLayout != null;
        }

        private void OnAddLabel()
        {
            Label label = new Label
            {
                ProductName = $"ProductName{Labels.Count}",
                ProductNumber = "ProductNumber",
                Price = 0.00,
                TaxValue = 23
            };

            AddLabel(label);

        }

        private void AddLabel(Label label)
        {
            SelectedLabel = label;
            if (SelectedLabel.Fields.Count == 0)
            {
                foreach (Field field in SelectedLayout.Fields)
                {
                    Field tempField = new Field();
                    tempField.Set(field);
                    SelectedLabel.Fields.Add(tempField);
                }
            }
            Labels.Add(SelectedLabel);
            _selectedLabelNumber = Labels.Count;
        }

        private void OnGenerateSample()
        {
            Renderer renderer = new Renderer(
                new Layout("Sample", 0, 0, 70.0, 29.7, 2.0, 2.0));
            renderer.RenderSampleV2();
        }

        public void LoadLayouts()
        {
            ObservableCollection<Layout> layouts = LayoutListViewModel.Layouts;
            Layouts = layouts;
        }

        public bool IsMappingChanged(ObservableCollection<Field> fields1,
            ObservableCollection<Field> fields2)
        {
            if(fields1.Count != fields2.Count)
            {
                return true;
            }
            
            for(int i = 0; i < fields1.Count; i++)
            {
                if (fields1[i].Type == fields2[i].Type
                    && fields1[i].MappedColumn != null
                    && fields2[i].MappedColumn != null)
                {
                    if(fields1[i].MappedColumn.ColumnName == fields2[i].MappedColumn.ColumnName)
                    {
                        continue;
                    }
                }
                else if(fields1[i].Type == fields2[i].Type
                    && fields1[i].MappedColumn == fields2[i].MappedColumn)
                {
                    continue;
                }
                return true;
            }

            return false;
        }
    }
}
