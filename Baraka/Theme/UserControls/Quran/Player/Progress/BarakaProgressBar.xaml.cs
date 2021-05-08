using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Baraka.Utils;

namespace Baraka.Theme.UserControls.Quran.Player
{
    /// <summary>
    /// Logique d'interaction pour BarakaProgressBar.xaml
    /// </summary>
    public partial class BarakaProgressBar : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(double), typeof(MainWindow));

        #region Update
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void RaisePropertyChanged(string propertyName)
        {
            var handlers = PropertyChanged;

            handlers(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Settings
        [Category("Baraka")]
        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set
            {
                if (ProgressEllipse != null)
                {
                    var margin = 3 + border.ActualWidth * Progress - ProgressEllipse.ActualWidth / 2;
                    
                    while (3 + border.ActualWidth - margin < ProgressEllipse.ActualWidth)
                    {
                        margin--;
                    }

                    ProgressEllipse.Margin =
                        new Thickness(margin, 0, 0, 0);
                }

                SetValue(ProgressProperty, value);
                RaisePropertyChanged("Progress");
            }
        }
        #endregion

        #region Events
        [Category("Baraka")]
        public event EventHandler<double> CursorChanged;
        #endregion

        public BarakaProgressBar()
        {
            Progress = 0;

            InitializeComponent();

            DataContext = this;
        }

        #region Navigation
        // Drag
        private bool _isDragging = false;
        
        private void SetDragging(bool b)
        {
            if (b && !_isDragging)
            {
                ProgressEllipse.Stroke = Brushes.YellowGreen;
                ProgressEllipse.Height += 3;
            }
            else if (!b && _isDragging)
            {
                ProgressEllipse.Stroke = Brushes.Transparent;
                ProgressEllipse.Height -= 3;
            }

            _isDragging = b;
        }
        
        private void ProgressEllipse_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetDragging(true);
        }

        private void ProgressEllipse_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetDragging(false);
        }
        
        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SetDragging(false);
        }

        private void border_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Console.WriteLine($"mousemove {e.GetPosition(border)}");
            if (_isDragging)
            {
                SetEllipseX(e.GetPosition(border).X);
                CursorChanged?.Invoke(this, e.GetPosition(border).X / border.ActualWidth);
            }
        }

        // One-click navigate
        private void border_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                SetDragging(false);
            }


            SetEllipseX(e.GetPosition(border).X);
            CursorChanged?.Invoke(this, e.GetPosition(border).X / border.ActualWidth);
        }

        // Utils
        private void SetEllipseX(double x)
        {
            double margin = 3 + x - ProgressEllipse.ActualWidth / 2;
            while (margin > border.ActualWidth - ProgressEllipse.ActualWidth / 2 - 8)
            {
                margin--;
            }
            ProgressEllipse.Margin = new Thickness(margin, 0, 0, 0);
        }
        #endregion
    }
}