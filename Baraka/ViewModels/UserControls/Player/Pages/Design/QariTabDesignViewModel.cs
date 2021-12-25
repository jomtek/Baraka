using Baraka.Models;
using Baraka.Models.Quran;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.ViewModels.UserControls.Player.Pages.Design
{
    public class QariTabDesignViewModel : NotifiableBase
    {
        public ObservableCollection<QariModel> QariList
        {
            get {
                return new ObservableCollection<QariModel>()
                {
                    new QariModel("abbad", "Fares", "Abbad", ""),
                    new QariModel("alafasy", "Mishary bin Rashid", "Alafasy", ""),
                    new QariModel("minshawi_mujawwad", "Muhammad Siddiq", "Minshawi", "mujawwad"),
                    new QariModel("minshawi_murattal", "Muhammad Siddiq", "Minshawi", "murattal"),
                    new QariModel("french", "Youssouf", "Leclerc", ""),
                    new QariModel("jaber", "Ali", "Jaber", ""),
                    new QariModel("sudais", "Abderrahman", "Al-Sudais", ""),
                    new QariModel("yassin", "Sahl", "Yassin", ""),
                };
            }
        }

        public QariTabDesignViewModel()
        {}
    }
}
