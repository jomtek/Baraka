using Baraka.Data;
using Baraka.Data.Descriptions;
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

namespace Baraka.Theme.UserControls.Quran.Display.Mushaf
{
    /// <summary>
    /// Logique d'interaction pour BarakaMushafSurahDisplayer.xaml
    /// </summary>
    public partial class BarakaMushafSurahDisplayer : UserControl, ISurahDisplayer
    {
        private int _actualPage = -1;

        #region Settings
        public int ActualPage
        {
            get { return _actualPage; }
            set
            {
                if (value > 604)
                {
                    value = 603;
                }
                else if (value < 1)
                {
                    value = 1;
                }

                _actualPage = value;

                // Update UI
                CurrentPageTB.Text = $"Pages {value}-{value + 1}";
                LastPageBTN.IsEnabled = (value != 1);
                NextPageBTN.IsEnabled = (value != 603);

                // Apply
                RightPage.LoadPage(value);
                LeftPage.LoadPage(value + 1);
            }
        }
        #endregion

        public BarakaMushafSurahDisplayer()
        {
            InitializeComponent();
        }

        private void Run_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Run)sender).Foreground = Brushes.Yellow;
        }

        private void Run_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Run)sender).Foreground = Brushes.Black;
        }

        public async Task LoadSurahAsync(SurahDescription surah)
        {
            //throw new NotImplementedException();
            ((MainWindow)App.Current.MainWindow).ReportLoadingProgress(1);
        }

        public void UnloadActualSurah()
        {
        }

        private async void LeftPage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Debug purposes

        }

        #region Navigation
        public async Task NaturalBrowse(bool next, int factor = 1)
        {
            int actual = _actualPage;

            await Task.Delay(100);

            if (_actualPage == actual)
            {
                if (next)
                {
                    ActualPage += 2 * factor;
                }
                else
                {
                    ActualPage -= 2 * factor;
                }
            }
        }

        private void LastPageBTN_Click(object sender, RoutedEventArgs e)
        {
            ActualPage -= 2;
        }

        private void NextPageBTN_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"w: {this.ActualWidth}, h: {this.ActualHeight}");
            ActualPage += 2;
        }
        #endregion

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*
            LSpine.Width = LeftPage.GetHorizontalBorderWidth();
            RSpine.Width = RightPage.GetHorizontalBorderWidth();
            */
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        #region Focus and zoom
        public void ApplyScale(double scale)
        {
            if (LeftPage.IsMouseOver)
            {
                LeftPage.ApplyScale(scale);
            }
            else if (RightPage.IsMouseOver)
            {
                RightPage.ApplyScale(scale);
            }
        }
        #endregion
    }
}
