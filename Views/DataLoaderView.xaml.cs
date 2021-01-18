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
using System.Windows.Shapes;

namespace WPF_ProductLabel.Views
{
    /// <summary>
    /// Interaction logic for DataLoaderView.xaml
    /// </summary>
    public partial class DataLoaderView : Window
    {
        public ICommand SaveLoadedDataCommand
        {
            get
            {
                return (ICommand)GetValue(SaveLoadedDataCommandProperty);
            }
            set
            {
                SetValue(SaveLoadedDataCommandProperty, value);
            }
        }

        public static DependencyProperty SaveLoadedDataCommandProperty =
            DependencyProperty.Register("SaveLoadedDataCommand",
                typeof(ICommand),
                typeof(DataLoaderView),
                new PropertyMetadata(null));


        public DataLoaderView()
        {
            InitializeComponent();
            var binding = new Binding("SaveProductDataCommand");
            SetBinding(SaveLoadedDataCommandProperty, binding);
        }

        private void dataLoaderWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SaveLoadedDataCommand != null)
            {
                SaveLoadedDataCommand.Execute(null);
            }
        }
    }
}
