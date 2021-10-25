using Baraka.Utils.MVVM.ViewModel;
using Baraka.ViewModels.UserControls.Player.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.ViewModels.UserControls.Player
{
    public class PlayerViewModel : ViewModelBase
    {
        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
        }

        public PlayerViewModel()
        {
            CurrentPage = new QariTabViewModel();
            
        }
    }
}
