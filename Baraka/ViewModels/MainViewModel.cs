using Baraka.Models.State;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using Baraka.ViewModels.UserControls.Displayers.TextDisplayer;
using Baraka.ViewModels.UserControls.Player;
using Baraka.ViewModels.UserControls.Player.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Baraka.ViewModels
{
    public class MainViewModel : NotifiableBase
    {
        private double _displayerScale = 1.1;
        public double DisplayerScale
        {
            get { return _displayerScale; }
            set { _displayerScale = value; OnPropertyChanged(nameof(DisplayerScale)); }
        }

        public NotifiableBase DisplayerContext { get; }
        public PlayerViewModel PlayerContext { get; }
        public ICommand ZoomCommand { get; }
        public MainViewModel(TextDisplayerViewModel displayerVm, PlayerViewModel playerVm)
        {
            DisplayerContext = displayerVm;
            PlayerContext = playerVm;

            ZoomCommand = new RelayCommand((param) =>
            {
                if (param is int delta)
                {
                    if (delta > 0)
                    {
                        if (DisplayerScale < 1.65)
                        {
                            DisplayerScale += 0.15;
                        }
                    }
                    else
                    {
                        if (DisplayerScale > 1.1)
                        {
                            DisplayerScale -= 0.15;
                        }
                    }
                }
            });
        }

        public static MainViewModel Create(AppState app)
        {
            var displayerVm = TextDisplayerViewModel.Create(app);
            var playerVm = PlayerViewModel.Create(app);
            return new MainViewModel(displayerVm, playerVm);
        }
    }
}
