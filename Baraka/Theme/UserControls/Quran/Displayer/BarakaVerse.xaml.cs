using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using System.Windows;
using System.Windows.Controls;

namespace Baraka.Theme.UserControls.Quran.Displayer
{
    /// <summary>
    /// Logique d'interaction pour BarakaVerse.xaml
    /// </summary>
    public partial class BarakaVerse : UserControl
    {
        private SurahDescription _surah;
        private int _number;

        public BarakaVerse(SurahDescription surah, int number)
        {
            InitializeComponent();

            _surah = surah;
            _number = number;
        }

        public void Initialize()
        {
            SurahVersion[] versions = Data.LoadedData.SurahList[_surah];
            ArabicTB.Text = versions[0].Verses[_number];
            PhoneticTB.Text = versions[1].Verses[_number];
            TranslatedTB.Text = versions[2].Verses[_number];

            // Measure and arrange early so that the size is known from the initialization
            Measure(new Size(650, double.PositiveInfinity));
            Arrange(new Rect(0, 0, 650, DesiredSize.Height));
        }
    }
}