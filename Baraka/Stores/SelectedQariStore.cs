using Baraka.Models;
using Baraka.Models.Quran;
using Baraka.Singletons;
using Baraka.Utils.MVVM.ViewModel;
using System;

namespace Baraka.Stores
{
    public class SelectedQariStore
    {
        public event Action<QariModel> ValueChanged;

        public void ChangeSelectedQari(QariModel value)
        {
            AppStateSingleton.Instance.SelectedQari = value;
            ValueChanged?.Invoke(value);
        }
    }
}
