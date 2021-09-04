using Baraka.Data;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Baraka.Theme.UserControls.Quran.Display.Translated
{
    /// <summary>
    /// Logique d'interaction pour BarakaMarkbook.xaml
    /// </summary>
    public partial class BarakaBookmark : UserControl
    {
        public BarakaTranslatedSurahDisplayer Displayer { get; set; }

        public BarakaBookmark()
        {
            InitializeComponent();
        }

        public void SetLoopMode(bool loop)
        {
            if (loop)
            {
                TopPath.Stroke = (SolidColorBrush)FindResource("LoopMode_Stroke_Brush");
                TopPath.Fill = (SolidColorBrush)FindResource("LoopMode_Fill_Brush");
                DownPath.Stroke = (SolidColorBrush)FindResource("LoopMode_Stroke_Brush");
                DownPath.Fill = (SolidColorBrush)FindResource("LoopMode_Fill_Brush");
            }
            else
            {
                TopPath.Stroke = (SolidColorBrush)FindResource("DarkBrush");
                TopPath.Fill = (SolidColorBrush)FindResource("NormalMode_Fill_Brush");
                DownPath.Stroke = (SolidColorBrush)FindResource("DarkBrush");
                DownPath.Fill = (SolidColorBrush)FindResource("NormalMode_Fill_Brush");
            }
        }

        #region ContextMenu
        private void Menu_Download_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Displayer.DownloadRecitation();
        }

        private void Menu_CopyVerses_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var sb = new StringBuilder();
            for (int i = Displayer.StartVerse; i <= Displayer.EndVerse; i++)
            {
                sb.AppendLine(Utils.Quran.General.PrettyPrintVerse(i, Displayer.Surah));
            }

            Clipboard.SetText(sb.ToString());
            SystemSounds.Exclamation.Play();
        }
        #endregion
    }
}
