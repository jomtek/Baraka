using Baraka.Stores;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using Baraka.ViewModels.UserControls.Displayers.TextDisplayer;
using Baraka.ViewModels.UserControls.Player;
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
        public NotifiableBase PlayerContext { get; }
        public ICommand ZoomCommand { get; }
        public MainViewModel(SelectedSuraStore selectedSuraStore)
        {
            DisplayerContext = new TextDisplayerViewModel(selectedSuraStore);
            PlayerContext = new PlayerViewModel(selectedSuraStore);

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
                    System.Diagnostics.Trace.WriteLine(DisplayerScale);
                }
            });
        }
    }
}
