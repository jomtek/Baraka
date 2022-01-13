using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models
{
    public class AppSettingsModel : NotifiableBase
    {
        private int _crossfadingValue;
        public int CrossfadingValue
        {
            get { return _crossfadingValue; }
            set { _crossfadingValue = value; OnPropertyChanged(nameof(CrossfadingValue)); }
        }
    }
}
