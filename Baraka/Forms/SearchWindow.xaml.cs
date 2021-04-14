using Baraka.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using Baraka.Utils.Search;
using Baraka.Theme.UserControls.Quran.Searcher;
using System.ComponentModel;
using Baraka.Data.Descriptions;
using System.Windows.Media.Animation;

namespace Baraka.Forms
{
    /// <summary>
    /// Logique d'interaction pour SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        #region Events
        [Category("Baraka")]
        public event EventHandler<VerseDescription> VerseClicked;
        #endregion

        private bool _exitMode = false;

        public SearchWindow()
        {
            InitializeComponent();
        }

        public void ScrollToTop()
        {
            MainSB.ResetThumbY();
            ResultsSV.ScrollToTop();
        }

        public void ResultClicked(SearchResult sres)
        {
            VerseClicked?.Invoke(this, new VerseDescription(sres.Surah, sres.Verse + 1));
            //MessageBox.Show($"{sres.Verse} cliqué");
            _exitMode = true;
            Close();
        }

        private async void SearchTB_TextChanged(object sender, EventArgs e)
        {
            var query = General.PrepareQuery(SearchTB.Text);

            if (query.Length > 0)
            {
                await ProcessQuery(query);
            }
            else
            {
                ResultsStatsTB.Text = $"0 versets trouvés";
                ShowAllTB.Visibility = Visibility.Collapsed;
                ResultsSP.Children.Clear();
            }
        }

        private async Task ProcessQuery(string query, bool showAll = false)
        {
            await Task.Delay(250);

            if (query != General.PrepareQuery(SearchTB.Text))
            {
                return;
            }

            ScrollToTop();
            ResultsSP.Children.Clear();

            var results = new RelevantSearch().Go(query);

            if (results.Count > 50)
            {
                ShowAllTB.Visibility = Visibility.Visible;
            }
            else
            {
                ShowAllTB.Visibility = Visibility.Collapsed;
            }

            if (results.Count == 1)
            {
                ResultsStatsTB.Text = $"{results.Count} verset trouvé";
            }
            else if (results.Count < 100)
            {
                ResultsStatsTB.Text = $"{results.Count} versets trouvés";
            }
            else
            {
                ResultsStatsTB.Text = $"{results.Count} versets trouvés ({Math.Round(results.Count / 6236d, 2)}%)";
            }

            if (!showAll) // Reduce search latency
            {
                results = results.Take(50).ToList();
            }

            for (int i = 0; i < results.Count; i++)
            {
                var resultBar = new BarakaSearchResult(results[i], this)
                {
                    Margin = new Thickness(0, i==0?0:10, 0, 0)
                };
                
                ResultsSP.Children.Add(resultBar);
            }

            MainSB.TargetValue = results.Count;

            return;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResultsSV_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MainSB.Scrolled = ResultsSV.VerticalOffset / ResultsSV.ScrollableHeight;
        }

        private void MainSB_OnScroll(object sender, EventArgs e)
        {
            if (ResultsSV.ScrollableHeight * MainSB.Scrolled > 0)
            {
                ResultsSV.ScrollToVerticalOffset(ResultsSV.ScrollableHeight * MainSB.Scrolled);
            }
        }

        private void ShowAllTB_MouseEnter(object sender, MouseEventArgs e)
        {
            ShowAllTB.Background = new SolidColorBrush(SystemColors.ControlLightColor);
            
        }

        private void ShowAllTB_MouseLeave(object sender, MouseEventArgs e)
        {
            ShowAllTB.Background = Brushes.Transparent;
        }

        private async void ShowAllTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            using (new Utils.WaitCursor())
            {
                string query = General.PrepareQuery(SearchTB.Text);
                await ProcessQuery(query, true);
            }

            ShowAllTB.Visibility = Visibility.Collapsed;
        }

        private void ResultsStatsTB_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
