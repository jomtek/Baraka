using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Baraka.Commands;
using Baraka.Commands.Welcome;
using Baraka.Utils.MVVM.ViewModel;
using Baraka.Utils.UX.Interaction;

namespace Baraka.ViewModels.Splashes
{
    public class WelcomeViewModel : NotifiableBase
    {
        public ICommand LoadDataCommand { get; set; }

        public event Action ClosingRequest;
        
        private string _stateMessage;
        public string StateMessage
        {
            get { return _stateMessage; }
            set { _stateMessage = value; OnPropertyChanged(nameof(StateMessage)); }
        }

        public bool DoneLoading
        {
            set
            {
                if (value)
                    ClosingRequest?.Invoke();
            }
        }

        public WelcomeViewModel()
        {
            LoadDataCommand = new LoadDataCommand(this, HandleDataException);
        }

        private void HandleDataException(Exception ex)
        {
            new ErrorMessage("Une erreur est survenue lors du chargement de vos données...\n" +
                             $"Message: {ex.Message}").Show();
        }
    }
}
