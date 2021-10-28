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
    public class PlayerViewModel : ViewModelBase
    {
        private QariTabViewModel _qariTab;
        private SuraTabViewModel _suraTab;
        public Action<bool> PlayerOpenChanged;

        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
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

        private bool _playerOpened = true; // TODO: change to false by default
        public bool PlayerOpened
        {
            get { return _playerOpened; }
            set
            {
                if (!value)
                {
                    CurrentPage = null;
                }

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
        public PlayerViewModel()
        {
            // Stores
            var scrollStateStore = new ScrollStateStore();
            scrollStateStore.ValueChanged += (newValue) =>
            {
                ScrollState = newValue;
            };

            // Commands
            ScrollCommand = new RelayCommand((param) =>
            {
                scrollStateStore.ChangeScrollState(ScrollState);
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
            _qariTab = new QariTabViewModel(scrollStateStore);
            _suraTab = new SuraTabViewModel(scrollStateStore);
        }
    }
}
