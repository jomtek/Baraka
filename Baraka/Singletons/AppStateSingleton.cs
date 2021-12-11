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

        public AppStateSingleton()
        {
            SelectedSuraStore = new SelectedSuraStore();
            SelectedQariStore = new SelectedQariStore();
        }

        // Values
        private SuraModel _selectedSura;
        public SuraModel SelectedSura
        {
            get { return _selectedSura; }
            set
            {
                _selectedSura = value;
                OnPropertyChanged(nameof(SelectedSura));
            }
        }

        private QariModel _selectedQari;
        public QariModel SelectedQari
        {
            get { return _selectedQari; }
            set
            {
                _selectedQari = value;
                OnPropertyChanged(nameof(SelectedQari));
            }
        }

        // Stores
        public SelectedSuraStore SelectedSuraStore;
        public SelectedQariStore SelectedQariStore;
    }
}
