using Baraka.Models.Quran;
using Baraka.Utils.MVVM;
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
            SelectedSuraStore = new UniqueStore<SuraModel>(null);
            SelectedQariStore = new UniqueStore<QariModel>(null);
        }

        // Stores
        public UniqueStore<SuraModel> SelectedSuraStore { get; }
        public UniqueStore<QariModel> SelectedQariStore { get; }
    }
}
