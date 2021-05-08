using System.Windows.Controls;
using System.Windows.Media;

namespace Baraka.Theme.UserControls.Quran.Displayer
{
    /// <summary>
    /// Logique d'interaction pour BarakaMarkbook.xaml
    /// </summary>
    public partial class BarakaBookmark : UserControl
    {
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
    }
}
