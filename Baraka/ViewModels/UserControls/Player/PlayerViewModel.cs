using Baraka.Stores;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using Baraka.ViewModels.UserControls.Player.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Baraka.ViewModels.UserControls.Player
{
    public class PlayerViewModel : NotifiableBase
    {
        private QariTabViewModel _qariTab;
        private SuraTabViewModel _suraTab;
        public Action<bool> PlayerOpenChanged;

        private ScrollStateStore _scrollStateStore;

        private NotifiableBase _currentPage;
        public NotifiableBase CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));

                _scrollStateStore.ChangeScrollState(0);
            }
        }

        private bool _qariTabSelected = false;
        public bool QariTabSelected
        {
            get { return _qariTabSelected; }
            set
            {
                _qariTabSelected = value;
                OnPropertyChanged(nameof(QariTabSelected));
                
                if (!SuraTabSelected)
                {
                    PlayerOpened = value;
                }

                if (value)
                {
                    SuraTabSelected = false;
                    CurrentPage = _qariTab;
                }
            }
        }

        private bool _suraTabSelected = false;
        public bool SuraTabSelected
        {
            get { return _suraTabSelected; }
            set
            {
                _suraTabSelected = value;
                OnPropertyChanged(nameof(SuraTabSelected));
                
                if (!QariTabSelected)
                {
                    PlayerOpened = value;
                }

                if (value)
                {
                    QariTabSelected = false;
                    CurrentPage = _suraTab;
                }
            }
        }

        private bool _playerOpened = true;
        public bool PlayerOpened
        {
            get { return _playerOpened; }
            set
            {
                _playerOpened = value;
                PlayerOpenChanged?.Invoke(value);
                OnPropertyChanged(nameof(PlayerOpened));
            }
        }

        private double _scrollState;
        public double ScrollState
        {
            get { return _scrollState; }
            set
            {
                _scrollState = value;
                OnPropertyChanged(nameof(ScrollState));
            }
        }

        public ICommand ScrollCommand { get; }
        public ICommand QariTabSelectedCommand { get; }
        public ICommand SuraTabSelectedCommand { get; }
        public PlayerViewModel(SelectedSuraStore selectedSuraStore)
        {
            // Stores
            _scrollStateStore = new ScrollStateStore();
            _scrollStateStore.ValueChanged += (newValue) =>
            {
                ScrollState = newValue;
            };

            // Commands
            ScrollCommand = new RelayCommand((param) =>
            {
                _scrollStateStore.ChangeScrollState(ScrollState);
            });

            QariTabSelectedCommand = new RelayCommand((param) =>
            {
                QariTabSelected = !QariTabSelected;
            });

            SuraTabSelectedCommand = new RelayCommand((param) =>
            {
                SuraTabSelected = !SuraTabSelected;
            });

            // Tab
            _qariTab = new QariTabViewModel(_scrollStateStore);
            _suraTab = new SuraTabViewModel(_scrollStateStore, selectedSuraStore);

            SuraTabSelected = true; // The default tab on the player is the sura tab
        }
    }
}
