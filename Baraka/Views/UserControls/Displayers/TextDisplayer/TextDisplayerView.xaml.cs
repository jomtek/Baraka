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
        private ListBoxItem _selectedItem;
        private VirtualizingPanel _panel;

        public TextDisplayerView()
        {
            InitializeComponent();
        }


        // Computes the height of the bookmark according to the layout
        private void RefreshBookmarkHeight()
        {
            //Trace.WriteLine("refreshing bookmark height... more info:");
            //Trace.WriteLine($"old height: {Bookmark.ActualHeight}");
           // Bookmark.Height = _panel.GetItemOffset(_selectedItem) + 60;
           // if (Bookmark.Height == 60)
            {
                Console.WriteLine();
            }
            //Trace.WriteLine($"new height: {Bookmark.ActualHeight}");
        }

        private void ListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //Trace.WriteLine($"height: {Bookmark.ActualHeight}");

            // Synchronize ListBox with the bookmark
            //BookmarkSV.ScrollToVerticalOffset(e.VerticalOffset);
        

            if (_selectedItem != null)
            {
                RefreshBookmarkHeight();
            }
        }
        
        private void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Trace.WriteLine("preview mouse left button up");
            var lbItem = Graphics.FindParent<ListBoxItem>((DependencyObject)sender);
            _selectedItem = lbItem;

           

            if (_panel == null)
                _panel = Graphics.FindParent<VirtualizingPanel>(lbItem);
            RefreshBookmarkHeight();
        }

        private void Verse_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            Trace.WriteLine("request bring into view");
            //Trace.WriteLine($"old height: {Bookmark.ActualHeight}");
            /* Cancel the event in order to prevent the list from annoyingly
               scrolling whenever you change current verse */
            e.Handled = true;
        }
    }
}