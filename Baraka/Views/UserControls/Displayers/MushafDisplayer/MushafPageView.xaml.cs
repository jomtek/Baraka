using Baraka.Models.Quran.Mushaf;
using Baraka.Services.Quran.Mushaf;
using Baraka.ViewModels.UserControls.Displayers.MushafDisplayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Baraka.Views.UserControls.Displayers.MushafDisplayer
{
    /// <summary>
    /// Interaction logic for MushafPageView.xaml
    /// </summary>
    public partial class MushafPageView : ContentControl
    {
        private MushafPageViewModel _vm;
        private List<StackPanel> _stackPanels;
        
        public MushafPageView()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is MushafPageViewModel vm)
            {
                _vm = vm;
                _vm.DisplayRequested += MushafPageView_DisplayRequested;
            }
        }

        private void MushafPageView_DisplayRequested(List<StackPanel> controls)
        {
            if (_vm == null)
                throw new ArgumentException();

            LinesSP.Children.Clear();

            foreach (var sp in controls)
                LinesSP.Children.Add(sp);
        }

        private void ChildSP_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Trace.WriteLine($"new size is {e.NewSize}");
        }
    }
}
