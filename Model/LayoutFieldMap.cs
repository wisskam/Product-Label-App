using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_ProductLabel.StaticResources;

namespace WPF_ProductLabel.Model
{
    class LayoutFieldMap : BindableBase
    {
        private ObservableCollection<DataColumn> _columns;
        private ObservableCollection<DataColumn> _selectedColumns;
        private Dictionary<TypesEnum, DataColumn> _fieldMap;

        public ObservableCollection<Field> Fields { get; set; }
        public ObservableCollection<DataColumn> Columns { get => _columns; set => _columns = value; }
        public ObservableCollection<DataColumn> SelectedColumns { get => _selectedColumns; set => _selectedColumns = value; }
        public Dictionary<TypesEnum, DataColumn> FieldMap { get => _fieldMap; set => _fieldMap = value; }
        
        public LayoutFieldMap(ObservableCollection<Field> fields)
        {
            Fields = fields;
            FieldMap = new Dictionary<TypesEnum, DataColumn>();
            Columns = new ObservableCollection<DataColumn>();
            SelectedColumns = new ObservableCollection<DataColumn>();

            //GenerateEmptyFieldMap();
        }

        public void AddColumn(DataColumn column)
        {
            Columns.Add(column);
        }

        private void GenerateFieldMap()
        {
            foreach(Field field in Fields)
            {
                FieldMap.Add(field.Type, null);
            }
            OnPropertyChanged("FieldMap");
        }

    }
}
