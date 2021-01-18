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
    /// Interaction logic for LabelGeneratorView.xaml
    /// </summary>
    public partial class LabelGeneratorView : UserControl
    {
        public LabelGeneratorView()
        {
            InitializeComponent();
        }

        private void OnDataLoader_Click(object sender, RoutedEventArgs e)
        {
            DataLoaderView dataLoaderView = new DataLoaderView();
            dataLoaderView.DataContext = this.DataContext;
            dataLoaderView.Show();
        }
    }
}
