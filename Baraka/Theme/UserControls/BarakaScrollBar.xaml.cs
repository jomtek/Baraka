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
                    Reset();
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
            double defaultHeight = (ScrollGrid.ActualHeight / _targetValue);
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

            // Prevent thumb from getting too small
            if (newHeight < ScrollGrid.ActualHeight * 0.03)
            {
                newHeight = ScrollGrid.ActualHeight * 0.08;
            }
            else if (newHeight < ScrollGrid.ActualHeight * 0.08)
            {
                newHeight = ScrollGrid.ActualHeight * 0.12;
            }

            // Prevent thumb from getting too big
            if (newHeight > ScrollGrid.ActualHeight * 0.9)
            {
                newHeight = ScrollGrid.ActualHeight * 0.8;
            }
            else if (newHeight > ScrollGrid.ActualHeight * 0.7)
            {
                newHeight = ScrollGrid.ActualHeight * 0.6;
            }

            // Overrided values
            if (_targetValue == 1)
            {
                newHeight = ScrollGrid.ActualHeight;
            }

            // // //

            ThumbGrid.Height = newHeight;
        }
        #endregion

        public BarakaScrollBar()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            ChangeThumbTopMargin(-(ScrollGrid.ActualHeight - ThumbGrid.Height), true);
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
        private void ThumbGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDown = true;
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mouseDown = false;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                double newThumbTopMargin
                    = ThumbGrid.Margin.Top + Mouse.GetPosition(ThumbGrid).Y - (int)(ThumbGrid.ActualHeight / 2d);
                ChangeThumbTopMargin(newThumbTopMargin);
            }
        }
        #endregion

        #region Click Scroll
        private void BackgroundRect_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double mouseRelativeY
                = Mouse.GetPosition(ExactMiddle).Y;

            if (mouseRelativeY > 0)
            {
                ChangeThumbTopMargin(mouseRelativeY + ThumbGrid.Height);
            }
            else
            {
                ChangeThumbTopMargin(mouseRelativeY - ThumbGrid.Height);
            }
        }
        #endregion

        #region UI Utils
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

        private void ChangeThumbTopMargin(double newMargin, bool reset = false)
        {
            if (!reset)
            {
                if (newMargin + ThumbGrid.ActualHeight > ScrollGrid.ActualHeight)
                {
                    // Prevent thumb from trespassing bottom limit
                    ChangeThumbTopMargin(ScrollGrid.ActualHeight - ThumbGrid.ActualHeight);
                    return;
                }
                else if (newMargin - ThumbGrid.ActualHeight < -ScrollGrid.ActualHeight)
                {
                    // Prevent thumb from trespassing top limit
                    ChangeThumbTopMargin(-(ScrollGrid.ActualHeight - ThumbGrid.ActualHeight));
                    return;
                }
            }

            ThumbGrid.Margin = new Thickness(
                ThumbGrid.Margin.Left,
                newMargin,
                ThumbGrid.Margin.Right,
                ThumbGrid.Margin.Bottom
            );

            double scrollAreaHeight = ScrollGrid.ActualHeight - ThumbGrid.ActualHeight;
            double relativeThumbY = ThumbGrid.TransformToAncestor(ScrollGrid).Transform(new Point(0, 0)).Y;
            _scrolled = relativeThumbY / scrollAreaHeight;
            // todo: fix first click bug

            if (!reset)
            {
                OnScroll?.Invoke(this, EventArgs.Empty);
                Console.WriteLine($"onscroll invoked, scrolled: {ThumbGrid.Margin.Top}");
            }
        }
        #endregion
    }
}
