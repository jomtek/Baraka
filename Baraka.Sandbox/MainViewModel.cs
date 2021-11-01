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
        private TestViewModel _testViewContext;
        public TestViewModel TestViewContext
        {
            get { return _testViewContext; }
            set { _testViewContext = value; OnPropertyChanged(nameof(TestViewContext)); }
        }

        public MainViewModel()
        {
            TestViewContext = new TestViewModel();
        }
    }
}
