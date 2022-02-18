using Baraka.Commands.UserControls.Player;
using Baraka.Models;
using Baraka.Models.Quran;
using Baraka.Models.State;
using Baraka.Services.Streaming;
using Baraka.Utils.MVVM;
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
        private AppState _app;
        public AppState App
        {
            get { return _app; }
            set { _app = value; }
        }

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

        public UniqueStore<double> ScrollStateStore { get; }
        public ICommand ScrollCommand { get; }
        public ICommand SuraSelectedCommand { get; }
        public ICommand SearchCommand { get; }
        public SuraTabViewModel(UniqueStore<double> scrollStateStore, AppState app, BookmarkState bookmark, SoundStreamingService streamingService)
        {
            App = app;

            // Init sura list
            SuraList = Services.Quran.SuraInfoService.GetAll();
           
            // Stores and commands
            ScrollStateStore = scrollStateStore;
            ScrollStateStore.ValueChanged += () =>
            {
                ScrollState = ScrollStateStore.Value;
            };

            ScrollCommand = new RelayCommand((param) =>
            {
                ScrollStateStore.Value = ScrollState;
            });

            SuraSelectedCommand = new RelayCommand(
                (param) =>
                {
                    if (param is SuraModel sura)
                    {
                        App.SelectedSuraStore.Value = sura;

                        bookmark.GoToSura(sura);
                        streamingService.RefreshCursor();
                    }
                },
                (sura) =>
                {
                    return App.SelectedSuraStore.Value != (SuraModel)sura;
                }
            );

            App.SelectedSuraStore.ValueChanged += () =>
            {
                CollectionViewSource.GetDefaultView(SuraList).Refresh();
            };
            
            SearchCommand = new SearchCommand(this);
        }
    }
}