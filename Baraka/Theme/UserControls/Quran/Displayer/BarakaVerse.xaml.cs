using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Baraka.Theme.UserControls.Quran.Displayer
{
    /// <summary>
    /// Logique d'interaction pour BarakaVerse.xaml
    /// </summary>
    public partial class BarakaVerse : UserControl
    {
        public SurahDescription Surah { get; private set; }
        public int Number { get; private set; }

        public BarakaVerse(SurahDescription surah, int number)
        {
            InitializeComponent();

            Surah = surah;
            Number = number;
        }

        public void Initialize()
        {
            int verNum = Number;

            SurahVersion[] versions = Data.LoadedData.SurahList[Surah];
            ArabicTB.Text = versions[0].Verses[verNum];
            PhoneticTB.Text = versions[1].Verses[verNum];
            TranslatedTB.Text = versions[2].Verses[verNum];

            // Measure and pre-arrange so that the size is known from the initialization
            Measure(new Size(650, double.PositiveInfinity));
            Arrange(new Rect(0, 0, 650, DesiredSize.Height));
        }
    }
}