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

        public static DependencyObject GetScrollViewer(DependencyObject o)
        {
            // Return the DependencyObject if it is a ScrollViewer
            if (o is ScrollViewer)
            { return o; }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);

                var result = GetScrollViewer(child);
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }
            return null;
        }

        private bool _blockNext = false;
        private void ListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_blockNext)
            {
                _blockNext = false;
                e.Handled = true;
                return;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                var sv = GetScrollViewer(VersesLB) as ScrollViewer;
                sv.ScrollToVerticalOffset(sv.VerticalOffset - e.VerticalChange);
                _blockNext = true;
                e.Handled = true;
                return;
            }
            
            BookmarkSV.ScrollToVerticalOffset(e.VerticalOffset);
        }
    }
}
