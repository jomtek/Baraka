using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Baraka.Theme.UserControls
{
    /// <summary>
    /// Logique d'interaction pour BarakaScrollBar.xaml
    /// </summary>
    public partial class BarakaScrollBar : UserControl
    {
        private bool _mouseDown = false;

        private double _scrolled = 0;
        private int _targetValue = 10;
        private ScrollAccuracyMode _accuracy = ScrollAccuracyMode.MEDIUM;

        [Category("Baraka")]
        public event EventHandler OnScroll;

        #region Settings
        [Category("Baraka")]
        public double Scrolled
        {
            get { return _scrolled; }
            set
            {
                if (value > 1)
                {
                    Scrolled = 1;
                    return;
                }

                _scrolled = value;
                SetThumbY(ScrollCanvas.ActualHeight * value, true);
            }
        }

        [Category("Baraka")]
        public int TargetValue
        {
            get { return _targetValue; }
            set
            {
                if (value != 0)
                {
                    _targetValue = value;
                    SetThumbHeight();
                }
            }
        }

        [Category("Baraka")]
        public ScrollAccuracyMode Accuracy
        {
            get { return _accuracy; }
            set
            {
                _accuracy = value;
                SetThumbHeight();
            }
        }

        public void SetThumbHeight()
        {
            double defaultHeight = (ScrollCanvas.ActualHeight / _targetValue);
            double newHeight;

            switch (_accuracy)
            {
                case ScrollAccuracyMode.VAGUE:
                    newHeight = defaultHeight * 6;
                    break;
                case ScrollAccuracyMode.MEDIUM:
                    newHeight = defaultHeight * 3.5;
                    break;
                case ScrollAccuracyMode.ACCURATE:
                    newHeight = defaultHeight * 1.5;
                    break;
                case ScrollAccuracyMode.SO_ACCURATE:
                    newHeight = defaultHeight;
                    break;
                default:
                    throw new NotImplementedException();
            }

            double scrollableHeight = ScrollCanvas.ActualHeight;

            // Prevent thumb from getting too small
            if (newHeight < scrollableHeight * 0.03)
            {
                newHeight = scrollableHeight * 0.08;
            }
            else if (newHeight < scrollableHeight * 0.08)
            {
                newHeight = scrollableHeight * 0.12;
            }

            // Prevent thumb from getting too big
            if (newHeight > scrollableHeight * 0.9)
            {
                newHeight = scrollableHeight * 0.8;
            }
            else if (newHeight > scrollableHeight * 0.7)
            {
                newHeight = scrollableHeight * 0.6;
            }

            // Overrided values
            if (_targetValue == 1)
            {
                newHeight = scrollableHeight;
            }

            // // //

            ThumbGrid.Height = newHeight;
            ResetThumbY();
        }
        #endregion

        public BarakaScrollBar()
        {
            InitializeComponent();
        }

        public void ResetThumbY()
        {
            SetThumbY(0, true);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int margin = DeductLeftRightMarginForThumb();
            ThumbRect.Margin = new Thickness(margin, 0, margin, 0);
            ThumbLinesStackPanel.Margin = new Thickness(2 * margin, 0, 2 * margin, 0);
            BackgroundRect.RadiusX = 3 * margin;
            BackgroundRect.RadiusY = 3 * margin;
            ThumbRect.RadiusX = 3 * margin;
            ThumbRect.RadiusY = 3 * margin;

            SetThumbHeight();
        }

        #region Manual Scroll
        double mouseOffset = 0;
        private void ThumbGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Define offset
            if (mouseOffset == 0) mouseOffset = ComputeMouseOffset();

            _mouseDown = true;
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mouseDown = false;
            mouseOffset = 0; // Reset offset
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown
                && Mouse.LeftButton == MouseButtonState.Pressed) // Prevent unexpected scrolls
            {
                double newThumbTop
                    = Mouse.GetPosition(ScrollCanvas).Y - mouseOffset;

                SetThumbY(newThumbTop);
            }
        }
        #endregion

        #region Click Scroll
        private void BackgroundRect_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetThumbY(Canvas.GetTop(ThumbGrid) + ComputeMouseOffset());
        }
        #endregion

        #region UI Utils
        private double ComputeMouseOffset()
        {
            return Mouse.GetPosition(ScrollCanvas).Y - Canvas.GetTop(ThumbGrid);
        }

        private int DeductLeftRightMarginForThumb()
        {
            if (Width > 25)
            {
                return 4;
            }
            else if (Width > 20)
            {
                return 3;
            }
            else if (Width > 13)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        private double _oldScrolled = 0;
        private void SetThumbY(double newY, bool reset = false)
        {
            double maxThumbY =
                ScrollCanvas.ActualHeight - ThumbGrid.ActualHeight;

            if (newY < 0)
            {
                SetThumbY(0);
                return;
            }
            else if (newY > maxThumbY)
            {
                SetThumbY(maxThumbY, reset);
                return;
            }

            Canvas.SetTop(ThumbGrid, newY);

            _scrolled = Canvas.GetTop(ThumbGrid) / maxThumbY;

            if (!reset
                && _scrolled != _oldScrolled) // Smooth event firings
            {
                OnScroll?.Invoke(this, EventArgs.Empty);
                _oldScrolled = _scrolled;
            }
        }
        #endregion
    }
}