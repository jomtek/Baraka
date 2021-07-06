using Baraka.Streaming;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Baraka.Theme.UserControls.Quran.Displayer
{
    /// <summary>
    /// Logique d'interaction pour BarakaVerseNumber.xaml
    /// </summary>
    public partial class BarakaVerseNumber : UserControl
    {
        private bool _playing = false;
        private int _num;
        private BarakaSurahDisplayer _displayer;

        public bool Playing
        {
            get { return _playing; }
            set
            {
                _playing = value;
                
                if (value)
                {
                    PolygonPath.Stroke = Brushes.Goldenrod;
                }
                else
                {
                    PolygonPath.Stroke = Brushes.Transparent;
                }
            }
        }

        // Debug
        public BarakaVerseNumber()
        {
            InitializeComponent();
        }

        public BarakaVerseNumber(BarakaSurahDisplayer displayer, int number, int displayNum, bool basmala = false)
        {
            InitializeComponent();

            _num = number;
            _displayer = displayer;

            if (!basmala)
            {
                NumberTB.Text = displayNum.ToString();
            }
            else
            {
                NumberTB.Visibility = Visibility.Collapsed;
                BasmalaEllipse.Visibility = Visibility.Visible;
            }
        }

        public void MarkAsSearchResult()
        {
            PolygonPath.Stroke = Brushes.YellowGreen;
        }

        #region Handlers
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            PolygonPath.Stroke = Brushes.Goldenrod;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_playing)
            {
                PolygonPath.Stroke = Brushes.Transparent;
            }
        }

        private void UserControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _displayer.VerseNum_Click(_num);
        }
        #endregion

        #region ContextMenu
        private void Menu_MoveHere_Click(object sender, RoutedEventArgs e)
        {
            _displayer.VerseNum_Click(_num);
        }

        private void Menu_StartHere_Click(object sender, RoutedEventArgs e)
        {
            _displayer.StartFromVerse(_num);
        }

        private void Menu_Download_Click(object sender, RoutedEventArgs e)
        {
            _displayer.DownloadRecitation(_num, _num);
        }

        private void Menu_CopyVerse_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }
        #endregion
    }
}
