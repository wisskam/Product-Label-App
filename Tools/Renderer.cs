using BarcodeLib;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using WPF_ProductLabel.Model;

using System;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Windows;
using WPF_ProductLabel.StaticResources;
using PdfSharp.Drawing.Layout;

namespace WPF_ProductLabel.Tools
{
    public class Renderer
    {
        private XFont _font;
        private XFont _fontBoldLarge;
        private XFont _fontBold;
        private PdfDocument _document;
        private PdfPage _page;
        private XGraphics _gfx;
        private Layout _layout;
        private CultureInfo _cultureInfo;
        private int _currentLabel;
        private int _possibleRows;
        private int _possibleColumns;
        private int _possibleLabelsOnPage;

        public Renderer(Layout layout)
        {
            if (layout == null)
            {
                throw new ArgumentNullException();
            }
            _layout = layout;
            // Create a new PDF document
            _document = new PdfDocument();
            _document.Info.Title = "Product Label Generator";
            // Create an empty page
            _page = _document.AddPage();

            _font = new XFont(_layout.FontFamily,
                _layout.FontSize
                , XFontStyle.Regular);
            _fontBoldLarge = new XFont(_layout.FontFamily,
                _layout.FontSize + 1,
                XFontStyle.Bold);
            _fontBold = new XFont(_layout.FontFamily,
                _layout.FontSize,
                XFontStyle.Bold);

            // Get an XGraphics object for drawing and add to the list
            _gfx = XGraphics.FromPdfPage(_page);
            // Prepare fonts
            XFont font = new XFont(_layout.FontFamily,
                _layout.FontSize
                , XFontStyle.Regular);
            XFont fontBoldLarge = new XFont(_layout.FontFamily,
                _layout.FontSize + 1,
                XFontStyle.Bold);
            XFont fontBold = new XFont(_layout.FontFamily,
                _layout.FontSize,
                XFontStyle.Bold);

            _cultureInfo = new CultureInfo(_layout.CountryISO);
            _currentLabel = 0;
            _possibleRows = _layout.CalculateRows(_layout.LabelHeight);
            _possibleColumns = _layout.CalculateColumns(_layout.LabelWidth);
            _possibleLabelsOnPage = _possibleRows * _possibleColumns;
        }
        public void RenderElement(XGraphics xGraphics, XPoint point, Field field, Label label)
        {
            XFont font = new XFont(field.FontFamilyText,
                            field.FontSize,
                            (XFontStyle)Enum.Parse(typeof(XFontStyle), field.WeightText));

            XRect rect = new XRect(
                point.X,
                point.Y,
                field.Width,
                field.Height
            );

            XTextFormatter xTextFormatter = new XTextFormatter(xGraphics);

            switch (field.TextAlignment)
            {
                case TextAlignmentEnum.Center:
                    {
                        xTextFormatter.Alignment = XParagraphAlignment.Center;
                        break;
                    }
                case TextAlignmentEnum.Right:
                    {
                        xTextFormatter.Alignment = XParagraphAlignment.Right;
                        break;
                    }
                default:
                    {
                        xTextFormatter.Alignment = XParagraphAlignment.Left;
                        break;
                    }
            }

            if(field.Value == null || field.Value == "")
            {
                field.Value = " ";
            }

            switch (field.Type)
            {
                case TypesEnum.ProductName:
                    {
                        xTextFormatter.DrawString(
                            field.Value,
                            font,
                            XBrushes.Black,
                            rect,
                            XStringFormats.TopLeft
                        );
            
                        break;
                    }
                case TypesEnum.ProductNumber:
                    {
                        xTextFormatter.DrawString(
                            field.Value,
                            font,
                            XBrushes.Black,
                            rect,
                            XStringFormats.TopLeft
                        );
                        break;
                    }
                case TypesEnum.Price:
                    {
                        xTextFormatter.DrawString(
                            $"{field.Value:C}",
                            font,
                            XBrushes.Black,
                            rect,
                            XStringFormats.TopLeft
                        );
                        break;
                    }
                case TypesEnum.TaxInfo:
                    {
                        Field taxValue = label.Fields.First(x => x.Type == TypesEnum.TaxValue);
                        if (taxValue != null)
                        {
                            string prepTextInfo = field.Value.Replace(
                                    "{VAT}", taxValue.Value);
                            xTextFormatter.DrawString(
                                prepTextInfo,
                                font,
                                XBrushes.Black,
                                rect,
                                XStringFormats.TopLeft
                            );

                        }
                        break;
                    }
                case TypesEnum.Barcode:
                    {
                        try
                        {
                            // Draw the barcode
                            Barcode barcode = new Barcode(
                                field.Value,
                                field.Width,
                                field.Height,
                                field.FontSize
                            );
                            Image image = barcode.GetImage();
                            Debug.WriteLine(
                                $"Image dimensions: {image.Width} width, {image.Height} height");
                            MemoryStream strm = new MemoryStream();
                            image.Save(strm, System.Drawing.Imaging.ImageFormat.Png);
                            xGraphics.DrawImage(
                                XImage.FromStream(strm),
                                point);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message,
                                "Error during preparing barcode!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                        break;
                    }
                case TypesEnum.Unit:
                    {
                        xTextFormatter.DrawString(
                            field.Value,
                            font,
                            XBrushes.Black,
                            rect,
                            XStringFormats.TopLeft
                        );
                        break;
                    }
            }

        }
        public void RenderNextLabel2(Label label)
        {

            int column = _currentLabel % _possibleColumns;
            int row = (int)Math.Ceiling(
                (double)((_currentLabel % _possibleLabelsOnPage) / _possibleColumns));

            XRect rect = new XRect((column * _layout.LabelWidth) + _layout.PageMarginX,
                       (row * _layout.LabelHeight) + _layout.PageMarginY,
                       _layout.LabelWidth,
                       _layout.LabelHeight);

            _gfx.DrawRectangle(XPens.Black, rect);
            int yOffset = 0;

            foreach (Field field in label.Fields)
            {
                XPoint xPoint = new XPoint(
                    rect.X + _layout.LabelMarginX,
                    rect.Y + _layout.LabelMarginY + yOffset
                );
                RenderElement(_gfx, xPoint, field, label);
                yOffset += field.Height;
            }
            _currentLabel++;
        }

        public void RenderNextLabel(Label label)
        {
            int column = _currentLabel % _possibleColumns;
            int row = (int)Math.Ceiling(
                (double)((_currentLabel % _possibleLabelsOnPage) / _possibleColumns));

            XRect rect = new XRect((column * _layout.LabelWidth) + _layout.PageMarginX,
                       (row * _layout.LabelHeight) + _layout.PageMarginY,
                       _layout.LabelWidth,
                       _layout.LabelHeight);
            Debug.WriteLine($"{_currentLabel} label position: {rect.X} X, {rect.Y} Y");
            Debug.WriteLine($"{_currentLabel} label: {column} Column, {row} Row");
            // Prepare points
            XPoint itemIdPoint = new XPoint(
                rect.X + _layout.LabelMarginX,
                rect.Y + _layout.LabelMarginY);
            XPoint itemNamePoint = new XPoint(
                itemIdPoint.X,
                itemIdPoint.Y + _font.Size);
            XPoint barcodePoint = new XPoint(
                rect.X + 10,
                rect.Bottom - 35);
            XPoint itemPricePoint = new XPoint(
                itemNamePoint.X + 30,
                itemNamePoint.Y + (barcodePoint.Y - itemIdPoint.Y) / 2);
            XPoint itemTaxPoint = new XPoint(
                itemPricePoint.X + (5 * _font.Size / 1.5),
                itemPricePoint.Y);
            // Draw the label frame
            _gfx.DrawRectangle(XPens.Black, rect);
            // Draw the itemId
            if (_layout.EnableProductNumber)
            {
                _gfx.DrawString(label.ProductNumber,
                    _fontBold,
                    XBrushes.Black,
                    itemIdPoint,
                    XStringFormats.TopLeft);
            }
            // Draw the item name
            if (_layout.EnableProductName)
            {
                _gfx.DrawString(label.ProductName,
                _font,
                XBrushes.Black,
                itemNamePoint,
                XStringFormats.TopLeft);
            }
            // Draw price
            if (_layout.EnablePrice)
            {
                string itemPrice = $"{label.Price:C}";
                _gfx.DrawString(itemPrice,
                _fontBoldLarge,
                XBrushes.Black,
                itemPricePoint,
                XStringFormats.TopLeft);
            }
            // Draw tax info
            if (_layout.EnableTaxInfo)
            {
                string itemTaxInfo = _layout.TaxInfo.Replace("{VAT}", label.TaxValue.ToString());
                _gfx.DrawString(itemTaxInfo,
                _font,
                XBrushes.Black,
                itemTaxPoint,
                XStringFormats.TopLeft);
            }
            if (_layout.EnableBarcode)
            {
                try
                {
                    // Draw the barcode
                    Barcode barcode = new Barcode(label.ProductNumber, 500, 25);
                    Image image = barcode.GetImage();
                    Debug.WriteLine($"Image dimensions: {image.Width} width, {image.Height} height");
                    MemoryStream strm = new MemoryStream();
                    image.Save(strm, System.Drawing.Imaging.ImageFormat.Png);
                    _gfx.DrawImage(
                        XImage.FromStream(strm),
                        barcodePoint);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                        "Error during preparing barcode!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            _currentLabel++;
        }

        public void SaveAndOpen()
        {
            // Save and open document 
            string filePath = Path.Combine(
                Layout.UserPath, $"ProductLabels_{DateTime.Now.ToString("ddMMyyyy")}.pdf");
            _document.Save(filePath);
            Process.Start(filePath);
        }

        public void RenderSampleV2()
        {
            for(int i =0; i < 10; i++)
            {
                RenderNextLabel(new Label
                {
                    ProductName = "Klawiatura hiper super",
                    ProductNumber = "KHS339922",
                    TaxValue = 23,
                    Price = 69.99
                });
            }
            SaveAndOpen();
        }
    }
}
