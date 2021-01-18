using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_ProductLabel.Views
{
    /// <summary>
    /// Interaction logic for LabelPreview.xaml
    /// </summary>
    public partial class LabelPreview : UserControl
    {
        public LabelPreview()
        {
            InitializeComponent();
        }

        //public void RenderImage()
        //{
        //    BarcodeLib.Barcode barcode = new BarcodeLib.Barcode("123asdzxc");
        //    Image image = barcode.GetImage();
        //    DrawingVisual drawingVisual = new DrawingVisual();
        //    DrawingContext drawingContext = drawingVisual.RenderOpen();
        //    drawingContext.DrawLine(new Pen(Brushes.Black, 10), new Point(0, 0), new Point(100, 100));
        //    drawingContext.Close();

        //    RenderTargetBitmap bmp = new RenderTargetBitmap(180, 180, 120, 96, PixelFormats.Pbgra32);
        //    bmp.Render(drawingVisual);
        //    image.Source = bmp;

        //    this.imageStackPanel.Children.Add(image);

        //}
    }
}
