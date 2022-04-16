using Baraka.Models.Quran.Mushaf;
using Baraka.Services.Quran.Mushaf;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Baraka.ViewModels.UserControls.Displayers.MushafDisplayer
{
    public class MushafDisplayerViewModel : NotifiableBase
    {
        public MushafPageViewModel LeftPageVm { get; set; }
        public MushafPageViewModel RightPageVm { get; set; }

        private double _fontSize = 13.5;
        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                try
                {
                    if (value < 35)
                    {
                        LeftPageVm.FontSize = value;
                        RightPageVm.FontSize = value;
                        _fontSize = value;
                    }

                    Trace.WriteLine($"fontsize: {value}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                }
            }
        }

        public double PageWidth
        {
            set
            {
                LeftPageVm.SetPageWidth(value);
                RightPageVm.SetPageWidth(value);
            }
        }

        public ICommand TurnPageLeftCommand { get; set; }
        public ICommand TurnPageRightCommand { get; set; }
        public MushafDisplayerViewModel()
        {
            TurnPageLeftCommand = new RelayCommand((param) =>
            {
                SetPage(LeftPageVm.Page + 2);
            });

            TurnPageRightCommand = new RelayCommand((param) =>
            {
                SetPage(LeftPageVm.Page - 2);
            });

            LeftPageVm = new MushafPageViewModel(1);
            RightPageVm = new MushafPageViewModel(0);
            FontSize = 13.5;
        }

        public static Task<T> StartSTATask<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            var thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        public void SetPage(int page)
        {
            LeftPageVm.SetPage(page);
            RightPageVm.SetPage(page - 1);

            Trace.WriteLine($"current page: {page}");
        }
    }
}
