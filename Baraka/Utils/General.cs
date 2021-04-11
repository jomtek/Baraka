using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace Baraka.Utils
{
    public static class General
    {
        public static void BindWidth(this FrameworkElement bindMe, FrameworkElement toMe)
        {
            Binding b = new Binding();
            b.Mode = BindingMode.OneWay;
            b.Source = toMe.ActualWidth;
            bindMe.SetBinding(FrameworkElement.WidthProperty, b);
        }
    }
}
