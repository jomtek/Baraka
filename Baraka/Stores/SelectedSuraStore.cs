using Baraka.Models;
using Baraka.Models.Quran;
using Baraka.Singletons;
using Baraka.Utils.MVVM.ViewModel;
using System;

namespace Baraka.Stores
{
    public class SelectedSuraStore : NotifiableBase
    {
        public event Action<SuraModel> ValueChanged;
        
        private SuraModel _value;
        public SuraModel Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(nameof(Value)); }
        }

        public void ChangeSelectedSura(SuraModel value)
        {
            AppStateSingleton.Instance.CurrentlyDisplayedSura = value;
            ValueChanged?.Invoke(value);
            Value = value;
        }
    }
}
