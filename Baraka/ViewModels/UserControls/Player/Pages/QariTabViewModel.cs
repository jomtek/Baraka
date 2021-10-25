using Baraka.Models;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.ViewModels.UserControls.Player.Pages
{
    public class QariTabViewModel : ViewModelBase
    {
        private ObservableCollection<QariModel> _qariList;
        public ObservableCollection<QariModel> QariList
        {
            get { return _qariList; }
            set { _qariList = value; }
        }

        public QariTabViewModel()
        {
            QariList = new ObservableCollection<QariModel>();
            foreach (var qari in Services.QariInfoService.GetAll())
            {
                QariList.Add(qari);
            }
        }
    }
}
