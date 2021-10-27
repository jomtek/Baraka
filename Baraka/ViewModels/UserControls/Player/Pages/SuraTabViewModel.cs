using Baraka.Models;
using Baraka.Stores;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        private double _scrollState = 0;
        public double ScrollState
        {
            get { return _scrollState; }
            set
            {
                if (value != _scrollState)
                {
                    _scrollState = value;
                    OnPropertyChanged(nameof(ScrollState));
                }
            }
        }

        public ICommand ScrollCommand { get; }
        public SuraTabViewModel(ScrollStateStore scrollStateStore)
        {
            SuraList = new ObservableCollection<SuraModel>();
            foreach (var sura in Services.Quran.SuraInfoService.GetAll())
            {
                SuraList.Add(sura);
            }

            scrollStateStore.ValueChanged += (newValue) =>
            {
                ScrollState = newValue;
            };

            ScrollCommand = new RelayCommand((param) =>
            {
                scrollStateStore.ChangeScrollState(ScrollState);
            });
        }
    }
}
