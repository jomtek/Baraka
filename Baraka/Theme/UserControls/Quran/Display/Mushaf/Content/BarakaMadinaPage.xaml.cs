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
using System.Data.SQLite;
using System.Data;
using Dapper;
using Baraka.Data.Descriptions;
using Baraka.Data;
using System.Net;
using System.IO;
using Baraka.Theme.UserControls.Quran.Display.Mushaf.Data;
using System.Diagnostics; // temporary

namespace Baraka.Theme.UserControls.Quran.Display.Mushaf.Content
{
    public enum MadinaPageSide
    {
        LEFT,
        RIGHT
    }

    /// <summary>
    /// Logique d'interaction pour BarakaMadinaPage.xaml
    /// </summary>
    public partial class BarakaMadinaPage : UserControl, INotifyPropertyChanged
    {
        private int _page = -1;
        private MadinaPageSide _side;

        private List<Grid> _surahTransitionItems;

        #region PropertyChanged Notifier
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Settings
        [Category("Baraka")]
        public MadinaPageSide Side
        {
            get { return _side; }
            set
            {
                if (value == MadinaPageSide.LEFT)
                {
                    Spine.HorizontalAlignment = HorizontalAlignment.Right;
                    Spine.Margin = new Thickness(0, Spine.Margin.Top, -50, Spine.Margin.Bottom);
                    SpineShadow.Direction = 180;
                }
                else if (value == MadinaPageSide.RIGHT)
                {
                    Spine.HorizontalAlignment = HorizontalAlignment.Left;
                    Spine.Margin = new Thickness(-50, Spine.Margin.Top, 0, Spine.Margin.Bottom);
                    SpineShadow.Direction = 360;
                }

                _side = value;
                
                RaisePropertyChanged("Side");
            }
        }
        #endregion

        public BarakaMadinaPage(int page)
        {
            InitializeComponent();
            _surahTransitionItems = new List<Grid>();

            Console.WriteLine($"load page {page}");
            LoadPage(page);
        }

        #region Core
        private void PrepareContainerGrid(int lines)
        {
            LinesContainerGrid.RowDefinitions.Clear();
            for (int i = 0; i < lines; i++)
            {
                var rd = new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star),
                };
                LinesContainerGrid.RowDefinitions.Add(rd);
            }
        }

        private void AddQuranicLine(List<MushafGlyphDescription> line, FontFamily family, int lineIndex, bool isAnnotation = false, bool allowHover = true)
        {
            var lineTB = new TextBlock();
            lineTB.FontFamily = family;

            // Build line
            foreach (MushafGlyphDescription glyph in line)
            {
                var run = new Run(glyph.DecodedData.ToString());

                switch (glyph.Type)
                {
                    case MushafGlyphType.STOPPING_SIGN:
                        run.Foreground = Brushes.CornflowerBlue;
                        break;
                    case MushafGlyphType.SUJOOD:
                    case MushafGlyphType.RUB_EL_HIZB:
                        break;
                    case MushafGlyphType.END_OF_AYAH:
                        /*
                        // TODO suggestion : Replace the end-of-ayah glyphs with a custom mark
                        var mark = new Image();
                        mark.Height = PageViewbox.ActualHeight / 25d;
                        mark.Source = new BitmapImage(new Uri("pack://application:,,,/Baraka;component/Images/ayah_num.png"));
                        PageTB.Inlines.Add(mark);
                        continue;
                        */
                        run.Foreground = Brushes.DodgerBlue;
                        break;
                    default: // Word
                        if (allowHover)
                        {
                            run.MouseEnter += (object sender, MouseEventArgs e) =>
                            {
                                run.Background = Brushes.LightGray;
                            };

                            run.MouseLeave += (object sender, MouseEventArgs e) =>
                            {
                                run.Background = Brushes.Transparent;
                            };
                        }
                        //_wordInlines.Add(run);
                        break;
                }

                lineTB.Inlines.Add(run);
            }

            // Pack line in a Viewbox and add it to the container grid
            var vb = new Viewbox()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = double.NaN,
                Height = double.NaN,

            };

            vb.Child = lineTB;
            vb.SetValue(Grid.RowProperty, lineIndex);

            if (isAnnotation)
            {
                AnnotationHelperGrid.Children.Add(vb);
            }
            else
            {
                LinesContainerGrid.Children.Add(vb);
            }
        }

        // `page` starts at 1
        public void LoadPage(int page)
        {
            var sw = new Stopwatch();
            sw.Start();

            // Clear all the currently displayed words, symbols, transitions and annotations
            LinesContainerGrid.Children.Clear();
            AnnotationHelperGrid.Children.Clear();
            _surahTransitionItems.Clear();

            // Reset scrollviewer
            LinesContainerSV.ScrollToVerticalOffset(0);

            // Set line container capacity
            if (page == 1 || page == 2)
            {
                PrepareContainerGrid(7);
            }
            else
            {
                PrepareContainerGrid(15);
            }

            // Reset margin accordingly
            _page = page;
            UpdateTextMargin();

            // Prepare required font families
            FontFamily pageFamily = LoadedData.MushafFontManager.FindPageFontFamily(page);
            FontFamily basmalaFamily = LoadedData.MushafFontManager.FindPageFontFamily(0);

            // Prepare page-specific background
            if (page == 1 || page == 2) // Opening
            {
                ImageComponent.Source = new BitmapImage(new Uri(@"/Images/opening_background.png", UriKind.Relative));
            }
            else
            {
                ImageComponent.Source = new BitmapImage(new Uri(@"/Baraka;component/Images/default_background.png", UriKind.Relative));
            }

            // Browse through each line of the specified page
            // and fill the 15-rows (or 8-rows) grid
            int count = 0;
            foreach (List<MushafGlyphDescription> line in LoadedData.MushafGlyphProvider.RetrievePage(page))
            {
                // TODO: perhaps join the two following blocks ?
                if (page == 1 || page == 2)
                {
                    switch (line[0].Type)
                    {
                        case MushafGlyphType.SURA_NAME:
                            AddQuranicLine(line, basmalaFamily, 3, true, false);
                            count--; // Compensate the count because line is added on another grid
                            break;
                        case MushafGlyphType.BASMALA:
                            AddQuranicLine(line, basmalaFamily, count, false, false);
                            break;
                        default:
                            AddQuranicLine(line, pageFamily, count);
                            break;
                    }
                }
                else // Normal pages
                {
                    switch (line[0].Type)
                    {
                        case MushafGlyphType.SURA_NAME:
                            // If glyph 0 has transliteration "suura", glyph 1 is the sura name
                            string fullName = new string(new char[] { line[0].DecodedData, line[1].DecodedData });
                            
                            var transitionBar = new BarakaMushafSurahTransition(fullName, line[0].AssociatedVerse)
                            {
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                            };

                            var containerGrid = new Grid();
                            containerGrid.Children.Add(transitionBar);
                            containerGrid.SetValue(Grid.RowProperty, count);
                            
                            LinesContainerGrid.Children.Add(containerGrid);
                            _surahTransitionItems.Add(containerGrid);
                            break;
                        case MushafGlyphType.BASMALA:
                            AddQuranicLine(line, basmalaFamily, count, false, false);
                            break;
                        default:
                            AddQuranicLine(line, pageFamily, count);
                            break;
                    }
                }

                count++;
            }

            // Re-apply the scale
            ApplyScale(ScaleTransformer.ScaleX, true);

            sw.Stop();
      
        }
        #endregion

        #region Align stuff
        // This part defines the margin of the Viewbox in a mushaf page,
        // according to a preset ratio associated with the mushaf background.

        // At the moment, the values are hardcoded for one specific mushaf design.
        // Long term, they should be stored in a future class describing the current mushaf style.

        public double GetHorizontalBorderWidth()
        {
            if (_page == 1 || _page == 2)
            {
                return GridComponent.ActualWidth * (0.293); // 29.3% of container width
            }
            else
            {
                return GridComponent.ActualWidth * (0.054); // 5.4% of container width
            }
        }

        public double GetHorizontalBorderHeight()
        {
            if (_page == 1 || _page == 2)
            {
                return GridComponent.ActualHeight * (0.3525); // 5.4% of container height
            }
            else
            {
                return GridComponent.ActualHeight * (0.054); // 5.4% of container height
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Spine.Margin = new Thickness(Spine.Margin.Left, GetHorizontalBorderHeight(), Spine.Margin.Right, GetHorizontalBorderWidth());
            UpdateTextMargin();
        }

        private void UpdateTextMargin()
        {
            double horizontalMargin = GetHorizontalBorderWidth();
            double topMargin = GetHorizontalBorderHeight();
            double bottomMargin = GetHorizontalBorderHeight();

            if (_page == 1 || _page == 2) // Opening
            {
                horizontalMargin += ImageComponent.ActualWidth * 0.05; // 5% horizontal offset
                topMargin += ImageComponent.ActualHeight * 0.02; // 2% vertical offset
            }

            LinesContainerSV.Margin = new Thickness(horizontalMargin, topMargin, horizontalMargin, bottomMargin);
        }
        #endregion

        #region Zoom

        public void ApplyScale(double scale, bool artificial = false)
        {
            // I don't know why, but the mushaf seems to start zooming only at 1.75 scale
            if (scale != 1 && !artificial)
                scale += 0.75;
            
            ScaleTransformer.ScaleX = scale;
            ScaleTransformer.ScaleY = scale;
            
            // Prevent the sura transition bars from growing too much
            foreach (Grid item in _surahTransitionItems)
            {
                item.LayoutTransform = LinesContainerGrid.LayoutTransform.Inverse as Transform;
            }
        }
        private void LinesContainerSV_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                return;
            }
        }
        #endregion

        #region Effects
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            LinesContainerSV.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            LinesContainerSV.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
        }
        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            LinesContainerSV.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            LinesContainerSV.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }
        #endregion
    }
}
