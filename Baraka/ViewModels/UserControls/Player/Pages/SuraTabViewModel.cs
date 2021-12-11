using Baraka.Commands.UserControls.Player;
using Baraka.Models;
using Baraka.Models.Quran;
using Baraka.Stores;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Baraka.ViewModels.UserControls.Player.Pages
{
    public class SuraTabViewModel : NotifiableBase, IScrollablePage
    {
        private IEnumerable<SuraModel> _suraList;
        public IEnumerable<SuraModel> SuraList
        {
            get { return _suraList; }
            set { _suraList = value; OnPropertyChanged(nameof(SuraList)); }
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

        private string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                
                SearchCommand.Execute(value);
            }
        }

        public ScrollStateStore ScrollStateStore { get; }
        public SelectedSuraStore SelectedSuraStore { get; }
        public ICommand ScrollCommand { get; }
        public ICommand SuraSelectedCommand { get; }
        public ICommand SearchCommand { get; }
        public SuraTabViewModel(ScrollStateStore scrollStateStore, SelectedSuraStore selectedSuraStore)
        {
            // Init sura list
            SuraList = Services.Quran.SuraInfoService.GetAll();
           
            // Stores and commands
            ScrollStateStore = scrollStateStore;
            ScrollStateStore.ValueChanged += (newValue) =>
            {
                ScrollState = newValue;
            };

            SelectedSuraStore = selectedSuraStore;

            ScrollCommand = new RelayCommand((param) =>
            {
                ScrollStateStore.ChangeScrollState(ScrollState);
            });

            SuraSelectedCommand = new RelayCommand(
                (sura) =>
                {
                    selectedSuraStore.ChangeSelectedSura((SuraModel)sura);
                    CollectionViewSource.GetDefaultView(SuraList).Refresh();
                },
                (sura) => {
                    return selectedSuraStore.Value != (SuraModel)sura;
                    }
            );
            
            SearchCommand = new SearchCommand(this);
        }
    }
}