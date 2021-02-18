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

namespace Baraka.Theme.UserControls.Player
{
    /// <summary>
    /// Logique d'interaction pour SurahBar.xaml
    /// </summary>
    public partial class SurahBar : UserControl
    {
        private Data.SurahDescription _surah;
        private BarakaPlayer _parentPlayer;

        #region Settings
        public Data.SurahDescription Surah
        {
            get { return _surah; }
        }
        #endregion

        public SurahBar(Data.SurahDescription surah, BarakaPlayer parent)
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
        }

        private void StreamBTN_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StreamBtnPath.Fill = Brushes.DarkGoldenrod;
            _parentPlayer.ChangeSelectedSurah(this);
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
                case Data.SurahRevelationType.M:
                    return "mecquoise (La Mecque)";
                case Data.SurahRevelationType.H:
                    return "médinoise (Médine)";
                default:
                    return "mecquoise ou médinoise";
            }
        }
        #endregion
    }
}
