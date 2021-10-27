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

        public ScrollStateStore ScrollStateStore;
        public PlayerViewModel()
        {
            // Stores
            ScrollStateStore = new ScrollStateStore();
            ScrollStateStore.ValueChanged += (newValue) =>
            {
                ScrollState = newValue;
            };

            // Commands
            ScrollCommand = new RelayCommand((param) =>
            {
                ScrollStateStore.ChangeScrollState(ScrollState);
            });

            // Tab
            _qariTab = new QariTabViewModel(ScrollStateStore);
            _suraTab = new SuraTabViewModel(ScrollStateStore);

            CurrentPage = _qariTab;
        }
    }
}
