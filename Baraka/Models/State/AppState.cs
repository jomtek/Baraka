using Baraka.Models.Quran;
using Baraka.Services;
using Baraka.Services.Quran;
using Baraka.Utils.MVVM;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models.State
{
    public class AppState : NotifiableBase
    {
        // Properties
        private AppSettingsModel _settings;
        public AppSettingsModel Settings
        {
            get { return _settings; }
            set { _settings = value; OnPropertyChanged(nameof(Settings)); }
        }

        // Stores
        public UniqueStore<SuraModel> SelectedSuraStore { get; set; }
        public UniqueStore<QariModel> SelectedQariStore { get; set; }

        public static AppState Create()
        {
            return new AppState()
            {
                Settings = AppSettingsModel.Create(),
                SelectedSuraStore = new UniqueStore<SuraModel>(SuraInfoService.FromNumber(1)), // Default sura
                SelectedQariStore = new UniqueStore<QariModel>(QariInfoService.GetAll()[1]),   // Default qari
            };
        }
    }
}
