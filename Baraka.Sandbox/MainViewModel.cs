using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Sandbox
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
        }

        public MainViewModel()
        {
            CurrentPage = new TestViewModel();
        }
    }
}
