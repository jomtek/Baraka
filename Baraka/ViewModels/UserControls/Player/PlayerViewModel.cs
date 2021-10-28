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

        private bool _qariTabSelected = false;
        public bool QariTabSelected
        {
            get { return _qariTabSelected; }
            set
            {
                _qariTabSelected = value;
                OnPropertyChanged(nameof(QariTabSelected));
                
                if (value)
                {
                    SuraTabSelected = false;
                    CurrentPage = _qariTab;
                }
                else if (!SuraTabSelected)
                {
                    ClosePlayer();
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
                
                if (value)
                {
                    QariTabSelected = false;
                    CurrentPage = _suraTab;
                }
                else if (!QariTabSelected)
                {
                    ClosePlayer();
                }
            }
        }

        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
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

        private void ClosePlayer()
        {
            CurrentPage = null;
        
        }
    }
}
