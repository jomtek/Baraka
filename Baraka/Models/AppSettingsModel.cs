using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models
{
    public class AppSettingsModel //: NotifiableBase
    {
        public int CrossfadingValue { get; set; }
        public int AudioCacheLength { get; set; }

        public static AppSettingsModel Create()
        {
            return new AppSettingsModel()
            {
                CrossfadingValue = 5000,
                AudioCacheLength = 10,
            };
        }
    }
}
