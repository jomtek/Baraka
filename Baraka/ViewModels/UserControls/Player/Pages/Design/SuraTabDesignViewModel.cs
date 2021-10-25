using Baraka.Models;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.ViewModels.UserControls.Player.Pages.Design
{
    public class SuraTabDesignViewModel : ViewModelBase
    {
        public ObservableCollection<SuraModel> SuraList
        {
            get
            {
                return new ObservableCollection<SuraModel>()
                {
                    new SuraModel(1,   7,    "Al-Fatiha",     "Prologue",                   SuraRevelationType.M),
                    new SuraModel(2,   286,  "Al-Baqarah",    "La vache",                   SuraRevelationType.H),
                    new SuraModel(3,   200,  "Al-Imran",      "La famille d'Imran",         SuraRevelationType.H),
                    new SuraModel(4,   176,  "An-Nisa",       "Les femmes",                 SuraRevelationType.H),
                    new SuraModel(5,   120,  "Al-Ma'ida",     "La table servie",            SuraRevelationType.H),
                    new SuraModel(6,   165,  "Al-Anam",       "Les bestiaux",               SuraRevelationType.M),
                    new SuraModel(7,   206,  "Al-Araf",       "Le purgatoire",              SuraRevelationType.M),
                    new SuraModel(8,   75,   "Al-Anfal",      "Le butin",                   SuraRevelationType.H),
                    new SuraModel(9,   129,  "At-Tawbah",     "Le repentir",                SuraRevelationType.H),
                    new SuraModel(10,  109,  "Yunus",         "Jonas",                      SuraRevelationType.M),
                    new SuraModel(11,  123,  "Hud",           "Hud",                        SuraRevelationType.M),
                    new SuraModel(12,  111,  "Yusuf",         "Joseph",                     SuraRevelationType.M),
                    new SuraModel(13,  43,   "Ar-Ra'd",       "Le tonnerre",                SuraRevelationType.M),
                    new SuraModel(14,  52,   "Ibrahim",       "Abraham",                    SuraRevelationType.M),
                    new SuraModel(15,  99,   "Al-Hijr",       "La vallée des pierres",      SuraRevelationType.M),
                    new SuraModel(16,  128,  "An-Nahl",       "Les abeilles",               SuraRevelationType.M),
                    new SuraModel(17,  111,  "Al-Isra",       "Le voyage nocturne",         SuraRevelationType.M),
                };
            }
        }

        public SuraTabDesignViewModel()
        {
            
        }
    }
}
