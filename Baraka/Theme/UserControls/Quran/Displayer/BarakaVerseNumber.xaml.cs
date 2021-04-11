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
        private int _num;
        private BarakaSurahDisplayer _displayer;

        public BarakaVerseNumber()
        {
            InitializeComponent();
        }

        public BarakaVerseNumber(BarakaSurahDisplayer displayer, int number, bool basmala = false)
        {
            InitializeComponent();

            _num = number;
            _displayer = displayer;

            if (!basmala)
            {
                NumberTB.Text = number.ToString();
            }
            else
            {
                NumberTB.Visibility = Visibility.Collapsed;
                BasmalaEllipse.Visibility = Visibility.Visible;
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            PolygonPath.Stroke = Brushes.Goldenrod;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            PolygonPath.Stroke = Brushes.Transparent;
        }

        private void UserControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _displayer.VerseNum_Click(_num);
        }

        private void Menu_MoveHere_Click(object sender, RoutedEventArgs e)
        {
            _displayer.VerseNum_Click(_num);
        }

        private void Menu_StartHere_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Menu_Download_Click(object sender, RoutedEventArgs e)
        {
            _displayer.DownloadMp3Verse(_num);
        }
    }
}
