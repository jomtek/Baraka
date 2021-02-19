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
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace Baraka.Theme.UserControls
{
    /// <summary>
    /// Logique d'interaction pour BarakaCheckBox.xaml
    /// </summary>
    public partial class BarakaCheckBox : UserControl
    {
        private bool _checked = true;

        #region Settings
        [Category("Baraka")]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                // todo
            }
        }
        #endregion

        public BarakaCheckBox()
        {
            InitializeComponent();
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            CheckEllipse.Fill = Brushes.Gainsboro;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            CheckEllipse.Fill = Brushes.White;
        }

        private void UserControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_checked)
            {
                ((Storyboard)this.Resources["UncheckStory"]).Begin();
            }
            else
            {
                ((Storyboard)this.Resources["CheckStory"]).Begin();
            }

            _checked = !_checked;
        }
    }
}
