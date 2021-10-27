using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Stores
{
    public class ScrollStateStore
    {
        public event Action<double> ValueChanged;

        public void ChangeScrollState(double value)
        {
            ValueChanged?.Invoke(value);
        }
    }
}
