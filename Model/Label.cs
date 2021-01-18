using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_ProductLabel.Model
{
    public class Label : BindableBase
    {
        private string _productName;
        private string _productNumber;
        private string _barcode;
        //private string _taxInfo;
        private int _taxValue;
        private double _price;
        private ObservableCollection<Field> _fields;

        public string ProductName { get => _productName; set => _productName = value; }
        public string ProductNumber { get => _productNumber; set => _productNumber = value; }
        //public string TaxInfo { get => _taxInfo; set => _taxInfo = value; }
        public int TaxValue { get => _taxValue; set => _taxValue = value; }
        public double Price { get => _price; set => _price = value; }
        public ObservableCollection<Field> Fields
        {
            get => _fields;
            set
            {
                _fields = value;
                OnPropertyChanged("Fields");
            }
        }

        public Label()
        {
            Fields = new ObservableCollection<Field>();
        }
    }
}
