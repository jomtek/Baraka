using Baraka.Models.UserControls;
using System;
using System.Collections.Generic;
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
        public List<DashboardItemModel> Items
        {
            get { return (List<DashboardItemModel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(List<DashboardItemModel>), typeof(DashboardView), new UIPropertyMetadata(null));
        
        public DashboardView()
        {
            InitializeComponent();
            Items = new List<DashboardItemModel>();
        }
    }
}