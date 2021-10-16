using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Baraka.Models.UserControls
{
    public class DashboardItemModel
    {
        public string Text { get; set; }
        public Style Icon { get; set; } // A Style you can find in App.xaml
        public ICommand Command { get; set; }
    }
}
