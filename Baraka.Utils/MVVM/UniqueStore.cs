using System;
using Baraka.Utils.MVVM.ViewModel;

namespace Baraka.Utils.MVVM
{
    public class UniqueStore<T> : NotifiableBase
    {
        public event Action ValueChanged;

        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
                ValueChanged?.Invoke();
            }
        }

        public UniqueStore(T defaultValue)
        {
            Value = defaultValue;
        }
    }
}
