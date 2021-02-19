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

namespace Baraka.Theme.UserControls.Quran.Player
{
    /// <summary>
    /// Logique d'interaction pour BarakaProgressBar.xaml
    /// </summary>
    public partial class BarakaProgressBar : UserControl
    {
        private double _progress = 0.5;

        #region Settings
        [Category("Baraka")]
        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                RefreshProgress();
            }
        }
        #endregion


        public BarakaProgressBar()
        {
            InitializeComponent();
        }

        private void RefreshProgress()
        {
            ProgressRect.Width = BackgroundRect.ActualWidth * _progress;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshProgress();
        }
    }
}
