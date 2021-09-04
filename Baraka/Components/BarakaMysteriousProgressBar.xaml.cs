using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Baraka.Theme.UserControls
{
    /// <summary>
    /// Logique d'interaction pour BarakaMysteriousProgressBar.xaml
    /// </summary>
    public partial class BarakaMysteriousProgressBar : UserControl
    {
        private double _progress = 0;

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

        public BarakaMysteriousProgressBar()
        {
            InitializeComponent();
        }

        private void RefreshProgress()
        {
            ProgressRect.Width = Math.Abs(35 + (BackgroundRect.ActualWidth - 37) * _progress);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshProgress();
        }
    }
}
