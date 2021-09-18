﻿using System;
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
using Baraka.Components.Quran.Display.Mushaf.Content;

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

        private List<Grid> _surahTransitionItems;

        #region PropertyChanged Notifier
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public BarakaMadinaPage(int page)
        {
            InitializeComponent();
            _surahTransitionItems = new List<Grid>();

            _page = page;
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

                        //run = new BarakaMushafEndOfAyah("a");

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
        public void LoadCurrentPage()
        {
            if (_page == -1)
                return;

            // Clear all the currently displayed words, symbols, transitions and annotations
            LinesContainerGrid.Children.Clear();
            AnnotationHelperGrid.Children.Clear();
            _surahTransitionItems.Clear();

            // Reset scrollviewer
            LinesContainerSV.ScrollToVerticalOffset(0);

            // Cancel and prevent any zoom on the first two pages
            if (_page == 1 || _page == 2)
            {
                GridComponent.Width = GlobalSV.ActualWidth;
                GridComponent.Height = GlobalSV.ActualHeight;
            }

            // Set line container capacity
            if (_page == 1 || _page == 2)
            {
                PrepareContainerGrid(7);
            }
            else
            {
                PrepareContainerGrid(15);
            }

            // Reset margin accordingly
            AlignTextAndAnnotations();

            // Prepare required font families
            FontFamily pageFamily = LoadedData.MushafFontManager.FindPageFontFamily(_page);
            FontFamily basmalaFamily = LoadedData.MushafFontManager.FindPageFontFamily(0);

            // Prepare page-specific background
            if (_page == 1 || _page == 2) // Opening
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
            foreach (List<MushafGlyphDescription> line in LoadedData.MushafGlyphProvider.RetrievePage(_page))
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

                        if (_page == 1 || _page == 2)
                        {
                            containerGrid.SetValue(Grid.RowProperty, 3);
                            AnnotationHelperGrid.Children.Add(containerGrid);

                            // Compensate the count because the line is added on another grid
                            count--;
                        }
                        else
                        {
                            containerGrid.SetValue(Grid.RowProperty, count);
                            LinesContainerGrid.Children.Add(containerGrid);
                        }
                            
                        _surahTransitionItems.Add(containerGrid);
                            
                        break;
                    case MushafGlyphType.BASMALA:
                        AddQuranicLine(line, basmalaFamily, count, false, false);
                        break;
                    default:
                        AddQuranicLine(line, pageFamily, count);
                        break;
                }

                count++;
            }

            // Reset the scale
            // and select a scrollviewer according to whether the page is part of the introduction of the mushaf or not
            if (_page <= 2)
            {
                ApplyScale(GlobalScaleTransformer.ScaleX);
                GlobalSV.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                GlobalSV.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            else
            {
                ApplyScale(LinesScaleTransformer.ScaleX);
                EnableDisableLinesContainerSV(true);
            }
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
                return GridComponent.ActualHeight * (0.3525); // 35.25% of container height
            }
            else
            {
                return GridComponent.ActualHeight * (0.054); // 5.4% of container height
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AlignTextAndAnnotations();
            if (_page == 1 || _page == 2)
            {
                ApplyScale(GlobalScaleTransformer.ScaleX);
            }
        }

        private void AlignTextAndAnnotations()
        {
            double horizontalMargin = GetHorizontalBorderWidth();
            double topMargin = GetHorizontalBorderHeight();
            double bottomMargin = GetHorizontalBorderHeight();

            AnnotationHelperGrid.Margin = new Thickness(horizontalMargin * 0.93, 0, horizontalMargin * 0.93, 0);

            if (_page == 1 || _page == 2) // Opening
            {
                horizontalMargin += ImageComponent.ActualWidth * 0.05; // 5% horizontal offset
                topMargin += ImageComponent.ActualHeight * 0.02; // 2% vertical offset
            }

            LinesContainerSV.Margin = new Thickness(horizontalMargin, topMargin, horizontalMargin, bottomMargin);
        }

        #endregion

        #region Zoom
        public void ApplyScale(double scale)
        {
            if (_page <= 2)
            {
                GlobalScaleTransformer.ScaleX = scale;
                GlobalScaleTransformer.ScaleY = scale;

                // Prevent the GridComponent from getting too small
                if (GridComponent.ActualWidth * scale < GlobalSV.ActualWidth)
                {
                    ApplyScale(scale + 0.005);
                }
            }
            else
            {
                LinesScaleTransformer.ScaleX = scale;
                LinesScaleTransformer.ScaleY = scale;
            }

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

        private void GlobalSV_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                return;
            }
        }
        #endregion

        #region Effects
        private void EnableDisableLinesContainerSV(bool enable)
        {
            if (enable)
            {
                LinesContainerSV.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                LinesContainerSV.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            else
            {
                LinesContainerSV.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                LinesContainerSV.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_page > 2)
            {
                EnableDisableLinesContainerSV(true);
            }
        }
        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_page > 2)
            {
                EnableDisableLinesContainerSV(false);
            }
        }
        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_page > 2)
            {
                EnableDisableLinesContainerSV(false);
            }
        }
        #endregion

        private void UserControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //GridComponent.Width = GlobalSV.ActualWidth;
            //GridComponent.Height = GlobalSV.ActualHeight;
        }
    }
}
