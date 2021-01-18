using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_ProductLabel.StaticResources;

namespace WPF_ProductLabel.Model
{
    public class Field : BindableBase
    {
        private TypesEnum _type;
        private int _width;
        private int _height;
        private WeightEnum _weight;
        private FontsEnum _fontFamily;
        private ColorEnum _color;
        private TextAlignmentEnum _textAlignmentt;
        private int _fontSize;
        private string _value;
        private DataColumn _mappedColumn;

        public TypesEnum Type { get => _type; set => _type = value; }
        public int Width { get => _width; set => _width = value; }
        public int Height { get => _height; set => _height = value; }
        public WeightEnum Weight { get => _weight; set => _weight = value; }
        public FontsEnum FontFamily { get => _fontFamily; set => _fontFamily = value; }
        public ColorEnum Color { get => _color; set => _color = value; }
        public TextAlignmentEnum TextAlignment { get => _textAlignmentt; set => _textAlignmentt = value; }
        public int FontSize { get => _fontSize; set => _fontSize = value; }
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public string FontFamilyText { get => _fontFamily.ToString(); }
        public string WeightText { get => _weight.ToString(); }
        public string ColorText { get => _color.ToString(); }
        public string TextAlignmentText { get => _textAlignmentt.ToString(); }
        public string TypeText { get => _type.ToString(); }
        public DataColumn MappedColumn { get => _mappedColumn; set => _mappedColumn = value; }

        public void Set(Field field)
        {
            Type = field.Type;
            Width = field.Width;
            Height = field.Height;
            Weight = field.Weight;
            FontFamily = field.FontFamily;
            FontSize = field.FontSize;
            Value = field.Value;
            MappedColumn = field.MappedColumn;
            Color = field.Color;
            TextAlignment = field.TextAlignment;
        }
    }
}
