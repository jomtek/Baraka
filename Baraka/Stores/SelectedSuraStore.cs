using Baraka.Models;
using System;

namespace Baraka.Stores
{
    public class SelectedSuraStore
    {
        public event Action<SuraModel> ValueChanged;

        public void ChangeSelectedSura(SuraModel value)
        {
            ValueChanged?.Invoke(value);
        }
    }
}
