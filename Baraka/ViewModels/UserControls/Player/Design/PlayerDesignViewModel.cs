using Baraka.Models.State;
using Baraka.Utils.MVVM.ViewModel;
using Baraka.ViewModels.UserControls.Player.Pages.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Baraka.ViewModels.UserControls.Player.Design
{
    public class PlayerDesignViewModel : NotifiableBase
    {
        public NotifiableBase CurrentPage { get; set; }
        public AppState App { get; set; }
        public bool QariTabSelected { get; set; } = false;
        public bool SuraTabSelected { get; set; } = true;
        public bool PlayerOpened { get; set; } = true;
        public double ScrollState { get; set; } = 0.0;
        public ICommand ScrollCommand { get; }
        public ICommand QariTabSelectedCommand { get; }
        public ICommand SuraTabSelectedCommand { get; }
        public ICommand NextSuraCommand { get; }
        public ICommand PreviousSuraCommand { get; }

        public PlayerDesignViewModel()
        {
            CurrentPage = new SuraTabDesignViewModel();
            App = AppState.CreateDesign();
        }
    }
}
