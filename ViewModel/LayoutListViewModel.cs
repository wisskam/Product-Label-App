using WPF_ProductLabel.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace WPF_ProductLabel.ViewModel
{
    class LayoutListViewModel : BindableBase
    {
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand DeleteFieldCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand LoadDetailsCommand { get; set; }

        private bool _detailsEnabled;
        private Layout _tempLayout;
        private Layout _selectedLayout;
        private Field _selectedField;

        public static ObservableCollection<Layout> Layouts { get; set; }

        public bool DetailsEnabled
        {
            get { return _detailsEnabled; }
            set
            {
                _detailsEnabled = value;
                OnPropertyChanged("DetailsEnabled");
            }
        }
        public Layout SelectedLayout
        {
            get { return _selectedLayout; }
            set
            {
                _selectedLayout = value;
                OnPropertyChanged("SelectedLayout");
            }
        }

        public Layout TempLayout
        {
            get { return _tempLayout; }
            set
            {
                _tempLayout = value;
                OnPropertyChanged("TempLayout");
            }
        }

        public Field SelectedField
        {
            get { return _selectedField; }
            set
            {
                _selectedField = value;
                OnPropertyChanged("SelectedField");
            }
        }

        public LayoutListViewModel()
        {
            LoadLayouts();

            DetailsEnabled = false;

            DeleteCommand = new RelayCommand(o => OnDelete(), o => CanDelete());
            DeleteFieldCommand = new RelayCommand(o => OnDeleteField(), o => CanDeleteField());
            SaveCommand = new RelayCommand(o => OnSave(), o => CanSave());
            AddCommand = new RelayCommand(o => OnAdd(), o => CanAdd());
            LoadDetailsCommand = new RelayCommand(o => OnLoadDetails(), o => CanLoadDetails());
        }

        private bool CanDeleteField()
        {
            return SelectedLayout != null && TempLayout != null && SelectedField != null;
        }

        private void OnDeleteField()
        {
            int fieldListIndex = TempLayout.Fields.IndexOf(SelectedField);
            TempLayout.Fields.Remove(SelectedField);
            SelectedField = null;
            if (TempLayout.Fields.Count - 1 > 0)
            {
                if(TempLayout.Fields.Count - 1 >= fieldListIndex)
                {
                    SelectedField = TempLayout.Fields[fieldListIndex];
                }
                else if (TempLayout.Fields.Count - 2 == fieldListIndex )
                {
                    SelectedField = TempLayout.Fields[fieldListIndex - 1];
                }
            }
        }

        public void LoadLayouts()
        {
            ObservableCollection<Layout> layouts = new ObservableCollection<Layout>();


            if (File.Exists(Layout.LayoutsPath))
            {
                try
                {
                    //Read saved layouts
                    layouts = Layout.ReadLayouts();
                }
                catch (Exception ex)
                {
                    //Throw message - tbi
                }
            }
            else
            {
                try
                {
                    //Add sample layouts
                    layouts.Add(new Layout("Full page 3x8", 0, 0, 70.0, 29.7, 2.0, 2.0));
                    Layout.SaveLayouts(layouts);

                }
                catch(Exception ex)
                {
                    //Throw message - tbi
                }
            }

            Layouts = layouts;
        }




        private void OnDelete()
        {
            try
            {
                Layouts.Remove(SelectedLayout);
                TempLayout = null;
                DetailsEnabled = false;
                Layout.SaveLayouts(Layouts);
            }
            catch
            {
                //Throw message - tbi
            }
        }

        private bool CanDelete()
        {
            return SelectedLayout != null;
        }
        private void OnSave()
        {
            try
            {
                SelectedLayout.Set(TempLayout);
                Layout.SaveLayouts(Layouts);
            }
            catch
            {
                //Throw message - tbi
            }
        }

        private bool CanSave()
        {
            return TempLayout != null;
        }
        private void OnAdd()
        {
            Layout newLayout = new Layout(
                $"Layout {Layouts.Count + 1}", 0, 0, 70.0, 29.7, 2.0, 2.0);
            Layouts.Add(newLayout);
            SelectedLayout = newLayout;
            TempLayout.Set(newLayout);
        }

        private bool CanAdd()
        {
            return true;
        }

        private bool CanLoadDetails()
        {
            return true;
        }

        private void OnLoadDetails()
        {
            DetailsEnabled = true;
            if (SelectedLayout != null)
            {
                TempLayout = new Layout();
                TempLayout.Set(SelectedLayout);
            }
        }
    }
}
