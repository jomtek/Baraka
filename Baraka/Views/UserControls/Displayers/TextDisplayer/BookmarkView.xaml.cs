using System;
using System.Collections.Generic;
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
    /// Interaction logic for BookmarkView.xaml
    /// </summary>
    public partial class BookmarkView : UserControl
    {
        public bool IsBeginning
        {
            get { return (bool)GetValue(IsBeginningProperty); }
            set { SetValue(IsBeginningProperty, value); }
        }

        public bool IsEnding
        {
            get { return (bool)GetValue(IsEndingProperty); }
            set { SetValue(IsEndingProperty, value); }
        }

        public bool IsOutspread
        {
            get { return (bool)GetValue(IsOutspreadProperty); }
            set { SetValue(IsOutspreadProperty, value); }
        }

        public bool IsLooping
        {
            get { return (bool)GetValue(IsLoopingProperty); }
            set { SetValue(IsLoopingProperty, value); }
        }

        public static readonly DependencyProperty IsBeginningProperty =
            DependencyProperty.Register("IsBeginning", typeof(bool), typeof(BookmarkView), new PropertyMetadata(false));

        public static readonly DependencyProperty IsEndingProperty =
            DependencyProperty.Register("IsEnding", typeof(bool), typeof(BookmarkView), new PropertyMetadata(false));

        public static readonly DependencyProperty IsOutspreadProperty =
            DependencyProperty.Register("IsOutspread", typeof(bool), typeof(BookmarkView), new PropertyMetadata(false));

        public static readonly DependencyProperty IsLoopingProperty =
            DependencyProperty.Register("IsLooping", typeof(bool), typeof(BookmarkView), new PropertyMetadata(false));

        public BookmarkView()
        {
            InitializeComponent();
        }
    }
}
