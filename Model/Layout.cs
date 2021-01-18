using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_ProductLabel.StaticResources;

namespace WPF_ProductLabel.Model
{
    public class Layout : BindableBase
    {
        private string _name;
        private readonly int _a4WidthMM = 210;
        private readonly int _a4HeightMM = 297;
        private string _fontFamily = "Arial";
        private int _fontSize = 10;
        private double _pageWidthMM;
        private double _pageHeightMM;
        private double _pageWidth = 595;
        private double _pageHeight = 842;
        private double _pageMarginY;
        private double _pageMarginYMM;
        private double _pageMarginX;
        private double _pageMarginXMM;
        private int _labelWidth;
        private int _labelHeight;
        private double _labelWidthMM;
        private double _labelHeightMM;
        private double _labelMarginY = 0;
        private double _labelMarginYMM = 0;
        private double _labelMarginX = 0;
        private double _labelMarginXMM = 0;
        private double _barcodeWidth;
        private double _barcodeMaxHeight;
        private string _taxInfo = "w tym {VAT}% vat";
        private string _countryISO = "pl-PL";
        private bool _autoMargin;
        private bool _enableProductName;
        private bool _enableProductNumber;
        private bool _enablePrice;
        private bool _enableTaxInfo;
        private bool _enableBarcode;
        private ObservableCollection<Field> _fields;

        private static string _userPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Documents",
            "ProductLabel"
        );
        private static string _fileName = "Layouts.json";

        public Layout() { }
        /*public Layout(
            string name,
            int pageMarginX,
            int pageMarginY,
            int labelWidth,
            int labelHeight,
            int labelMarginX,
            int labelMarginY)
        {
            Name = name;

            PageMarginX = pageMarginX;
            PageMarginY = pageMarginY;
            LabelWidth = labelWidth;
            LabelHeight = labelHeight;
            LabelMarginX = labelMarginX;
            LabelMarginY = labelMarginY;

            PageWidthMM = PageWidth / PxToMMWidthRatio;
            PageHeightMM = PageHeight / PxToMMHeightRatio;
            PageMarginXMM = pageMarginX / PxToMMWidthRatio;
            PageMarginYMM = pageMarginY / PxToMMHeightRatio;
            LabelWidthMM = labelWidth / PxToMMWidthRatio;
            LabelHeightMM = labelHeight / PxToMMHeightRatio;
            LabelMarginXMM = labelMarginX / PxToMMWidthRatio;
            LabelMarginYMM = labelMarginY / PxToMMHeightRatio;

            EnableBarcode = true;
            EnablePrice = true;
            EnableProductName = true;
            EnableProductNumber = true;
        }*/
        public Layout(
            string name,
            double pageMarginXMM,
            double pageMarginYMM,
            double labelWidthMM,
            double labelHeightMM,
            double labelMarginXMM,
            double labelMarginYMM)
        {
            Name = name;
            PageWidthMM = PageWidth / PxToMMWidthRatio;
            PageHeightMM = PageHeight / PxToMMHeightRatio;
            PageMarginXMM = pageMarginXMM;
            PageMarginYMM = pageMarginYMM;
            LabelWidthMM = labelWidthMM;
            LabelHeightMM = labelHeightMM;
            LabelMarginXMM = labelMarginXMM;
            LabelMarginYMM = labelMarginYMM;

            PageMarginX = pageMarginXMM * PxToMMWidthRatio;
            PageMarginY = pageMarginYMM * PxToMMHeightRatio;
            LabelWidth = (int)(labelWidthMM * PxToMMWidthRatio);
            LabelHeight = (int)(labelHeightMM * PxToMMHeightRatio);
            LabelMarginX = labelMarginXMM * PxToMMWidthRatio;
            LabelMarginY = labelMarginYMM * PxToMMHeightRatio;

            EnableTaxInfo = true;
            EnableBarcode = true;
            EnablePrice = true;
            EnableProductName = true;
            EnableProductNumber = true;

            Fields = new ObservableCollection<Field>();
            Fields.Add(new Field
            { 
                Width = 50, 
                Height = 15, 
                FontFamily = FontsEnum.Arial, 
                FontSize = 10,
                Weight = WeightEnum.Regular, 
                Type = TypesEnum.ProductName,
                Color = ColorEnum.LightBlue,
                TextAlignment = TextAlignmentEnum.Left,
                Value = "Nazwa produktu"
            });
            Fields.Add(new Field
            { 
                Width = 75, 
                Height = 20, 
                FontFamily = FontsEnum.Arial, 
                FontSize = 12, 
                Weight = WeightEnum.Bold,
                Type = TypesEnum.ProductNumber,
                Color = ColorEnum.LightCoral,
                TextAlignment = TextAlignmentEnum.Left,
                Value = "IND3K5"
            });
            Fields.Add(new Field
            { 
                Width = 100, 
                Height = 30, 
                FontFamily = FontsEnum.Arial, 
                FontSize = 15, 
                Weight = WeightEnum.Regular,
                Type = TypesEnum.Price,
                Color = ColorEnum.LightCyan,
                TextAlignment = TextAlignmentEnum.Left,
                Value = $"{0:C}"

            });

        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public int LabelWidth
        {
            get { return _labelWidth; }
            set
            {
                if (_labelWidth != value)
                {
                    _labelWidth = value;
                    OnPropertyChanged("Width");
                }
            }
        }
        public int LabelHeight
        {
            get { return _labelHeight; }
            set
            {
                if (_labelHeight != value)
                {
                    _labelHeight = value;
                    OnPropertyChanged("Height");
                }
            }
        }
        public string FontFamily
        {
            get { return _fontFamily; }
            set
            {
                if (_fontFamily != value)
                {
                    _fontFamily = value;
                    OnPropertyChanged("FontFamily");
                }
            }
        }
        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    OnPropertyChanged("FontSize");
                }
            }
        }
        public int RowHeight { get => (int)(Math.Round(FontSize * 1.2)); }


        public static string UserPath { get; set; } = _userPath;
        public static string FileName { get; set; } = _fileName;
        public static string LayoutsPath
        {
            get { return Path.Combine(_userPath, _fileName); }
        }

        public int A4WidthMM { get => _a4WidthMM; }
        public int A4HeightMM { get => _a4HeightMM; }
        public double PxToMMWidthRatio { get => _pageWidth / _a4WidthMM; }
        public double PxToMMHeightRatio { get => _pageHeight / _a4HeightMM; }
        public double PageWidth { get => _pageWidth; set => _pageWidth = value; }
        public double PageHeight { get => _pageHeight; set => _pageHeight = value; }
        public string CountryISO { get => _countryISO; set => _countryISO = value; }

        public double PageMarginY
        {
            get => _pageMarginY;
            set
            {
                _pageMarginY = value;
                OnPropertyChanged("PageMarginY");
            }
        }
        public double PageMarginX
        {
            get => _pageMarginX;
            set
            {
                _pageMarginX = value;
                OnPropertyChanged("PageMarginX");
            }
        }
        public double LabelMarginY
        {
            get => _labelMarginY;
            set
            {
                _labelMarginY = value;
                OnPropertyChanged("LabelMarginY");
            }
        }
        public double LabelMarginX
        {
            get => _labelMarginX;
            set
            {
                _labelMarginX = value;
                OnPropertyChanged("LabelMarginX");
            }
        }
        public double BarcodeWidth
        {
            get => _barcodeWidth;
            set
            {
                _barcodeWidth = value;
                OnPropertyChanged("BarcodeWidth");
            }
        }
        public double BarcodeMaxHeight
        {
            get => _barcodeMaxHeight;
            set
            {
                _barcodeMaxHeight = value;
                OnPropertyChanged("BarcodeMaxHeight");
            }
        }

        public double PageWidthMM
        {
            get => _pageWidthMM;
            set
            {
                _pageWidthMM = value;
                OnPropertyChanged("PageWidthMM");
            }
        }
        public double PageHeightMM
        {
            get => _pageHeightMM;
            set
            {
                _pageHeightMM = value;
                OnPropertyChanged("PageHeightMM");
            }
        }
        public double PageMarginYMM
        {
            get => _pageMarginYMM;
            set
            {
                _pageMarginYMM = value;
                OnPropertyChanged("PageMarginYMM");
            }
        }
        public double PageMarginXMM
        {
            get => _pageMarginXMM;
            set
            {
                _pageMarginXMM = value;
                OnPropertyChanged("PageMarginXMM");
            }
        }
        public double LabelWidthMM
        {
            get => _labelWidthMM;
            set
            {
                _labelWidthMM = value;
                OnPropertyChanged("LabelWidthMM");
            }
        }
        public double LabelHeightMM
        {
            get => _labelHeightMM;
            set
            {
                _labelHeightMM = value;
                OnPropertyChanged("LabelHeightMM");
            }
        }
        public double LabelMarginYMM
        {
            get => _labelMarginYMM;
            set
            {
                _labelMarginYMM = value;
                OnPropertyChanged("LabelMarginYMM");
            }
        }
        public double LabelMarginXMM
        {
            get => _labelMarginXMM;
            set
            {
                _labelMarginXMM = value;
                OnPropertyChanged("LabelMarginXMM");
            }
        }
        public bool EnableProductName
        {
            get => _enableProductName;
            set
            {
                _enableProductName = value;
                OnPropertyChanged("EnableProductName");
            }
        }
        public bool EnableProductNumber
        {
            get => _enableProductNumber;
            set
            {
                _enableProductNumber = value;
                OnPropertyChanged("EnableProductNumber");
            }
        }
        public bool EnablePrice
        {
            get => _enablePrice;
            set
            {
                _enablePrice = value;
                OnPropertyChanged("EnablePrice");
            }
        }
        public bool EnableTaxInfo
        {
            get => _enableTaxInfo;
            set
            {
                _enableTaxInfo = value;
                OnPropertyChanged("EnableTaxInfo");
            }
        }
        public bool EnableBarcode
        {
            get => _enableBarcode;
            set
            {
                _enableBarcode = value;
                OnPropertyChanged("EnableBarcode");
            }
        }
        public Thickness LabelMargin
        {
            get => new Thickness(_labelMarginY, _labelMarginX, _labelMarginY, _labelMarginX);
        }
        public string TaxInfo
        { 
            get => _taxInfo;
            set
            {
                _taxInfo = value;
                OnPropertyChanged("LabelMargin");
            }
        }
        public ObservableCollection<Field> Fields
        { 
            get => _fields;
            set
            {
                _fields = value;
                OnPropertyChanged("Fields");
            }
        }

        public void Set(Layout layout)
        {
            if (layout == null)
            {
                throw new ArgumentNullException();
            }
            this.Name = layout.Name;
            this.EnableProductName = layout.EnableProductName;
            this.EnableProductNumber = layout.EnableProductNumber;
            this.EnablePrice = layout.EnablePrice;
            this.EnableTaxInfo = layout.EnableTaxInfo;
            this.EnableBarcode = layout.EnableBarcode;
            this.CountryISO = layout.CountryISO;
            this.FontFamily = layout.FontFamily;
            this.FontSize = layout.FontSize;

            this.PageWidthMM = layout.PageWidthMM;
            this.PageHeightMM = layout.PageHeightMM;
            this.PageMarginYMM = layout.PageMarginYMM;
            this.PageMarginXMM = layout.PageMarginXMM;
            this.LabelWidthMM = layout.LabelWidthMM;
            this.LabelHeightMM = layout.LabelHeightMM;
            this.LabelMarginYMM = layout.LabelMarginYMM;
            this.LabelMarginXMM = layout.LabelMarginXMM;

            this.LabelWidth = (int)(layout.LabelWidthMM * this.PxToMMWidthRatio);
            this.LabelHeight = (int)(layout.LabelHeightMM * this.PxToMMHeightRatio);
            this.PageMarginY = (int)(layout.PageMarginYMM * this.PxToMMHeightRatio);
            this.PageMarginX = (int)(layout.PageMarginXMM * this.PxToMMWidthRatio);
            this.LabelMarginY = (int)(layout.LabelMarginYMM * this.PxToMMHeightRatio);
            this.LabelMarginX = (int)(layout.LabelMarginXMM * this.PxToMMWidthRatio);
            this.BarcodeWidth = layout.BarcodeWidth;
            this.BarcodeMaxHeight = layout.BarcodeMaxHeight;
            this.TaxInfo = layout.TaxInfo;

            Fields = new ObservableCollection<Field>();
            if(layout.Fields != null)
            {
                foreach (Field field in layout.Fields)
                {
                    Field tempField = new Field();
                    tempField.Set(field);
                    Fields.Add(tempField);
                }
            }

        }

        public static ObservableCollection<Layout> ReadLayouts()
        {
            ObservableCollection<Layout> layouts =
                JsonConvert.DeserializeObject<ObservableCollection<Layout>>(File.ReadAllText(LayoutsPath));
            return layouts;
        }

        public static void SaveLayouts(ObservableCollection<Layout> layouts)
        {
            using (StreamWriter file = File.CreateText(LayoutsPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, layouts);
            }
        }
        private static double GetRatio(double dimensionA, double dimensionB)
        {
            return dimensionB / dimensionA;
        }
        /// <summary>
        /// Returns possible amount of rows on page
        /// </summary>
        /// <param name="pageHeight">Page height in pt</param>
        /// <param name="labelHeight">Label height in pt</param>
        /// <returns></returns>
        public int CalculateRows(double labelHeight)
        {
            return (int)Math.Floor((PageHeight - 2 * PageMarginY) / labelHeight);
        }
        /// <summary>
        /// Returns possible amount of columns on page
        /// </summary>
        /// <param name="pageWidth">Page width in pt</param>
        /// <param name="labelWidth">Label width in pt</param>
        /// <returns></returns>
        public int CalculateColumns(double labelWidth)
        {
            return (int)Math.Floor((PageWidth - 2 * PageMarginX) / labelWidth);
        }


    }
}
