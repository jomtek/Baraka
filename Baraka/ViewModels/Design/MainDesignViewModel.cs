using Baraka.Utils.MVVM.ViewModel;
using Baraka.ViewModels.UserControls.Displayers.TextDisplayer.Design;
using Baraka.ViewModels.UserControls.Player.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.ViewModels.Design
{
    public class MainDesignViewModel : NotifiableBase
    {
        public NotifiableBase DisplayerContext { get; }
        public PlayerDesignViewModel PlayerContext { get; }

        public MainDesignViewModel()
        {
            DisplayerContext = new TextDisplayerDesignViewModel();
            PlayerContext = new PlayerDesignViewModel();
        }
    }
}
