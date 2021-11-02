using Baraka.Utils.MVVM.Command;
using Baraka.ViewModels.Splashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Baraka.Commands.Welcome
{
    public class LoadDataCommand : AsyncCommandBase
    {
        private readonly WelcomeViewModel _welcomeViewModel;

        public LoadDataCommand(WelcomeViewModel welcomeViewModel, Action<Exception> onException)
            : base(onException)
        {
            _welcomeViewModel = welcomeViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            //await Task.Delay(1000);
            _welcomeViewModel.DoneLoading = true;
        }
    }
}
