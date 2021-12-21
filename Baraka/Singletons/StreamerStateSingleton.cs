using Baraka.Models.Quran;
using Baraka.Stores;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Singletons
{
    public class StreamerStateSingleton : NotifiableBase
    {
        private static StreamerStateSingleton _instance;
        public static StreamerStateSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StreamerStateSingleton();
                }
                return _instance;
            }
        }

        public StreamerStateSingleton()
        {
            IsPlaying = false;
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get { return _isPlaying;; }
            set { _isPlaying = value; OnPropertyChanged(nameof(IsPlaying)); }
        }

    }
}
