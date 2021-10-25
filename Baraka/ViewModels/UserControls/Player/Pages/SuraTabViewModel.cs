using Baraka.Models;
using Baraka.Utils.MVVM.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.ViewModels.UserControls.Player.Pages
{
    public class SuraTabViewModel : ViewModelBase
    {
        private ObservableCollection<SuraModel> _suraList;
        public ObservableCollection<SuraModel> SuraList
        {
            get { return _suraList; }
            set { _suraList = value; }
        }

        public SuraTabViewModel()
        {
            SuraList = new ObservableCollection<SuraModel>();
            foreach (var sura in Services.Quran.SuraInfoService.GetAll())
            {
                SuraList.Add(sura);
            }
        }
    }
}
