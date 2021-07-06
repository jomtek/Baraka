using Baraka.Data;
using Baraka.Data.Descriptions;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Baraka.Theme.UserControls.Quran.Player
{
    /// <summary>
    /// Logique d'interaction pour SurahBar.xaml
    /// </summary>
    public partial class SurahBar : UserControl
    {
        private SurahDescription _surah;
        private BarakaPlayer _parentPlayer;

        #region Settings
        public SurahDescription Surah
        {
            get { return _surah; }
        }
        #endregion

        public SurahBar(SurahDescription surah, BarakaPlayer parent)
        {
            InitializeComponent();
            _surah = surah;
            Initialize();

            _parentPlayer = parent;
        }
        public void Initialize()
        {
            SurahNumberTB.Text = _surah.SurahNumber.ToString() + '.';
            SurahNameTB.Text = _surah.PhoneticName.ToString();
            TranslatedNameTB.Text = _surah.TranslatedName.ToString();
            InfoPath.ToolTip = $"Contient {_surah.NumberOfVerses} versets\nRévélation {DetermineRevelationType()}";

            RefreshProgress();
        }

        public void RefreshProgress()
        {
            int bookmark = LoadedData.Bookmarks[_surah.SurahNumber - 1];
            int progress = (int)Math.Floor(((double)bookmark / (_surah.NumberOfVerses - 1)) * 100);
            if (progress == 0)
            {
                ProgressTB.Visibility = Visibility.Collapsed;
            }
            else
            {
                ProgressTB.Text = $"{progress}%";
            }
        }

        private async void StreamBTN_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetLoading(true);
            await Task.Delay(5);
            _parentPlayer.SetDesignedBar(this);
            Select();
            SetLoading(false);
        }

        public void SetLoading(bool state)
        {
            if (state)
            {
                PlayControlsSP.Visibility = Visibility.Hidden;
                LoadingTB.Visibility = Visibility.Visible;
            }
            else
            {
                PlayControlsSP.Visibility = Visibility.Visible;
                LoadingTB.Visibility = Visibility.Hidden;
            }
        }

        public void Select()
        {
            StreamBtnPath.Fill = Brushes.DarkGoldenrod;
        }
        public void Unselect()
        {
            StreamBtnPath.Fill = Brushes.Black;
        }

        #region UI Utils
        private string DetermineRevelationType()
        {
            switch (_surah.RevelationType)
            {
                case SurahRevelationType.M:
                    return "mecquoise (La Mecque)";
                case SurahRevelationType.H:
                    return "médinoise (Médine)";
                default:
                    return "mecquoise ou médinoise";
            }
        }
        #endregion

        #region UI Reactivity
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            BottomSplitterPath.Stroke = (SolidColorBrush)App.Current.Resources["MediumBrush"];
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            BottomSplitterPath.Stroke = Brushes.LightGray;
        }
        #endregion
    }
}
