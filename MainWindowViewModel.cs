using WPF_ProductLabel.ViewModel;
using WPF_ProductLabel.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WPF_ProductLabel
{

    class MainWindowViewModel : BindableBase
    {

        public MainWindowViewModel()
        {
            NavCommand = new RelayCommand(obj => OnNav((string)obj), obj => CanNav());
            Directory.CreateDirectory(Layout.UserPath);
            //Default view
            CurrentViewModel = labelGeneratorViewModel;
        }

        private readonly LayoutListViewModel layoutListViewModel = new LayoutListViewModel();
        private readonly LabelGeneratorViewModel labelGeneratorViewModel = new LabelGeneratorViewModel();

        private BindableBase _CurrentViewModel;

        public BindableBase CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }
        }

        public RelayCommand NavCommand { get; private set; }

        private void OnNav(string destination)
        {

            switch (destination)
            {
                case "generator":
                    CurrentViewModel = labelGeneratorViewModel;
                    break;
                case "layout":
                default:
                    CurrentViewModel = layoutListViewModel;
                    break;
            }
        }
        private bool CanNav()
        {
            return true;
        }
    }
}