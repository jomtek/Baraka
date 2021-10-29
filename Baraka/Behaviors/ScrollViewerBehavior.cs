using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Baraka.Behaviors
{
    public static class ScrollViewerBehavior
    {

        /* This property allows you to directly bind a ScrollViewer to a double value representing its 
           scroll state */
        public static double GetScrollState(DependencyObject obj)
        {
            return (double)obj.GetValue(ScrollStateProperty);
        }

        public static void SetScrollState(DependencyObject obj, double value)
        {
            obj.SetValue(ScrollStateProperty, value);
        }

        public static readonly DependencyProperty ScrollStateProperty =
            DependencyProperty.RegisterAttached("ScrollState", typeof(double), typeof(ScrollViewerBehavior), new PropertyMetadata(0.5, (o, e) =>
            {
                var scrollViewer = o as ScrollViewer;

                if (scrollViewer != null)
                {
                    double newVO = (double)e.NewValue * scrollViewer.ScrollableHeight;
                    scrollViewer.ScrollToVerticalOffset(newVO);
                }
            }));

    }
}
