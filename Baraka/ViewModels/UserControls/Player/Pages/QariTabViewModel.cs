using Baraka.Models;
using Baraka.Stores;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Baraka.ViewModels.UserControls.Player.Pages
{
    public class QariTabViewModel : ViewModelBase, IScrollablePage
    {
        private ObservableCollection<QariModel> _qariList;
        public ObservableCollection<QariModel> QariList
        {
            get { return _qariList; }
            set { _qariList = value; }
        }

        private double _scrollState = 0;
        public double ScrollState
        {
            get { return _scrollState; }
            set
            {
                if (value != _scrollState)
                {
                    _scrollState = value;
                    OnPropertyChanged(nameof(ScrollState));
                }
            }
        }

        public ICommand ScrollCommand { get; }
        public QariTabViewModel(ScrollStateStore scrollStateStore)
        {
            QariList = new ObservableCollection<QariModel>();
            foreach (var qari in Services.QariInfoService.GetAll())
            {
                QariList.Add(qari);
            }

            scrollStateStore.ValueChanged += (newValue) =>
            {
                ScrollState = newValue;
            };

            ScrollCommand = new RelayCommand((param) =>
            {
                scrollStateStore.ChangeScrollState(ScrollState);
            });
        }
    }
}