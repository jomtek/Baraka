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
    public class QariTabDesignViewModel : ViewModelBase
    {
        public ObservableCollection<QariModel> QariList
        {
            get {
                return new ObservableCollection<QariModel>()
                {
                    new QariModel("abbad", "Fares", "Abbad"),
                    new QariModel("alafasy", "Mishary bin Rashid", "Alafasy"),
                    new QariModel("dosari", "Yasser", "Al-Dosari"),
                    new QariModel("french", "Youssouf", "Leclerc"),
                    new QariModel("jaber", "Ali", "Jaber"),
                    new QariModel("sudais", "Abderrahman", "Al-Sudais"),
                    new QariModel("yassin", "Sahl", "Yassin"),
                };
            }
            set { }
        }

        public QariTabDesignViewModel()
        {}
    }
}
