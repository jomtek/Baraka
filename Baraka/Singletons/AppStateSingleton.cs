using Baraka.Models.Quran;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Singletons
{
    public class AppStateSingleton : NotifiableBase
    {
        private static AppStateSingleton _instance;
        public static AppStateSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppStateSingleton();
                }
                return _instance;
            }
        }

        private SuraModel _currentlyDisplayedSura;
        public SuraModel CurrentlyDisplayedSura
        {
            get { return _currentlyDisplayedSura; }
            set
            {
                _currentlyDisplayedSura = value;
                OnPropertyChanged(nameof(CurrentlyDisplayedSura));
            }
        }


    }
}
