using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Sandbox
{
    public class MainViewModel : NotifiableBase
    {
        private ObservableCollection<string> _names;
        public ObservableCollection<string> Names
        {
            get { return _names; }
            set { _names = value; }
        }

private string selectedName;

public string SelectedName
{
    get { return selectedName; }
    set { selectedName = value; OnPropertyChanged(nameof(SelectedName)); }
}

        public MainViewModel()
        {
            Names = new ObservableCollection<string>()
            {
                "John", "Mauricio", "Franklin", "Isabelle"
            };

            SelectedName = "Mauricio";
        }
    }
}
