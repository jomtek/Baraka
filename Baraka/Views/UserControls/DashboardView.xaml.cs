using Baraka.Models.UserControls;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Baraka.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    
    [ContentProperty("Items")]
    public partial class DashboardView : UserControl
    {
        public ObservableCollection<DashboardItemModel> Items
        {
            get { return (ObservableCollection<DashboardItemModel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<Object>), typeof(DashboardView), new UIPropertyMetadata(new ObservableCollection<object>()));
        
        public DashboardView()
        {
            InitializeComponent();
        }
    }
}