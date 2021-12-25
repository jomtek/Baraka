using Baraka.Models;
using Baraka.Models.Quran;
using Baraka.Singletons;
using Baraka.Utils.MVVM;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Input;

namespace Baraka.ViewModels.UserControls.Player.Pages
{
    public class QariTabViewModel : NotifiableBase, IScrollablePage
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
        public ICommand QariSelectedCommand { get; }
        public QariTabViewModel(Store<double> scrollStateStore)
        {
            QariList = new ObservableCollection<QariModel>();
            foreach (var qari in Services.QariInfoService.GetAll())
            {
                QariList.Add(qari);
            }

            scrollStateStore.ValueChanged += () =>
            {
                ScrollState = scrollStateStore.Value;
            };

            ScrollCommand = new RelayCommand((param) =>
            {
                scrollStateStore.Value = ScrollState;
            });

            QariSelectedCommand = new RelayCommand(
                (qari) => {
                    AppStateSingleton.Instance.SelectedQariStore.Value = (QariModel)qari;
                    CollectionViewSource.GetDefaultView(QariList).Refresh();
                },
                (qari) => {
                    return AppStateSingleton.Instance.SelectedQariStore.Value != (QariModel)qari;
                }
            );
        }
    }
}