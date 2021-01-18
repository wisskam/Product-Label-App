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
using WPF_ProductLabel.Model;

namespace WPF_ProductLabel.Views
{
    /// <summary>
    /// Interaction logic for LayoutListView.xaml
    /// </summary>
    public partial class LayoutListView : UserControl
    {
        public ICommand LoadDetailsCommand
        {
            get
            {
                return (ICommand)GetValue(LoadDetailsCommandProperty);
            }
            set
            {
                SetValue(LoadDetailsCommandProperty, value);
            }
        }

        public static DependencyProperty LoadDetailsCommandProperty = 
            DependencyProperty.Register("LoadDetailsCommand",
                typeof(ICommand),
                typeof(LayoutListView),
                new PropertyMetadata(null));

        public LayoutListView()
        {
            InitializeComponent();
        }

        private void layoutListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(LoadDetailsCommand != null)
            {
                LoadDetailsCommand.Execute(null);
            }
        }
    }
}
