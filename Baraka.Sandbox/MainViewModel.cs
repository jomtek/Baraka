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
        private double _scrollValue = 0;
        public double ScrollValue
        {
            get { return _scrollValue; }
            set
            {
                // This code is never reached 
                _scrollValue = value;
                OnPropertyChanged(nameof(ScrollValue));
            }
        }
    }
}
