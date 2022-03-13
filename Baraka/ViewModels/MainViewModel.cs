using Baraka.Models.State;
using Baraka.Services.Streaming;
using Baraka.Utils.MVVM;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using Baraka.ViewModels.UserControls.Displayers.MushafDisplayer;
using Baraka.ViewModels.UserControls.Displayers.TextDisplayer;
using Baraka.ViewModels.UserControls.Player;
using Baraka.ViewModels.UserControls.Player.Design;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Baraka.ViewModels
{
    public class MainViewModel : NotifiableBase
    {
        public bool IsMushafSelected = true;

        private double _displayerScale = 1.2;
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
            if (!IsMushafSelected)
            {
                DisplayerContext = displayerVm;
            }
            else
            {
                DisplayerContext = new MushafDisplayerViewModel();
            }
            PlayerContext = playerVm;

            ZoomCommand = new RelayCommand((param) =>
            {
                if (param is int delta)
                {
                    if (delta > 0)
                    {
                        if (DisplayerScale < 2)
                        {
                            DisplayerScale += 0.15;
                        }
                    }
                    else
                    {
                        if (DisplayerScale >= 1.2 + 0.15)
                        {
                            DisplayerScale -= 0.15;
                        }
                    }

                    // Apply zoom scale to the mushaf displayer
                    // Our goal is to convert a scale to a mushaf font size
                    // We will achieve this by using the rule of three
                    // 1.2 scale     13.5 font size
                    // x   scale     ?    font size 
                    (DisplayerContext as MushafDisplayerViewModel).FontSize = (DisplayerScale * 13.5) / 1.2;
                }
            });
        }

        public static MainViewModel Create(AppState app, BookmarkState bookmark, SoundStreamingService streamingService)
        {
            var displayerVm = TextDisplayerViewModel.Create(app, bookmark, streamingService);
            var playerVm = PlayerViewModel.Create(app, bookmark, streamingService);
            return new MainViewModel(displayerVm, playerVm);
        }
    }
}
