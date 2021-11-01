using Baraka.Stores;
using Baraka.Utils.MVVM.ViewModel;
using Baraka.ViewModels.UserControls.Displayers.TextDisplayer;
using Baraka.ViewModels.UserControls.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelBase DisplayerContext { get; }
        public ViewModelBase PlayerContext { get; }

        public MainViewModel()
        {
            var selectedSuraStore = new SelectedSuraStore();
            DisplayerContext = new TextDisplayerViewModel(selectedSuraStore);
            PlayerContext = new PlayerViewModel(selectedSuraStore);
        }
    }
}
