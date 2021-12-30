using Baraka.Behaviors;
using Baraka.ViewModels.UserControls.Displayers;
using Baraka.ViewModels.UserControls.Displayers.TextDisplayer;
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
using Baraka.Utils.UI;

namespace Baraka.Views.UserControls.Displayers.TextDisplayer
{
    /// <summary>
    /// Interaction logic for TextDisplayerView.xaml
    /// </summary>
    public partial class TextDisplayerView : UserControl
    {
        public TextDisplayerView()
        {
            InitializeComponent();
        }

        private void Verse_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            /* Cancel the event in order to prevent the list from annoyingly
               scrolling whenever you change current verse */
            e.Handled = true;
        }
    }
}