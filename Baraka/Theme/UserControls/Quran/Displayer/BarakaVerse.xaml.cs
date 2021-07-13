using Baraka.Data;
using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using System;
using System.Collections.Generic;
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

            Dictionary<string, SurahVersion> versions = Data.LoadedData.SurahList[Surah];
            var config = Data.LoadedData.Settings.SurahVersionConfig;

            if (config.DisplayArabic)
            {
                ArabicTB.Text = versions["ARABIC"].Verses[verNum];
                ArabicTB.Visibility = Visibility.Visible;
            }
            else
            {
                ArabicTB.Visibility = Visibility.Collapsed;
            }

            if (config.DisplayPhonetic)
            {
                PhoneticTB.Text = versions["PHONETIC"].Verses[verNum];
                PhoneticTB.Visibility = Visibility.Visible;
            }
            else
            {
                PhoneticTB.Visibility = Visibility.Collapsed;
            }

            if (config.DisplayTranslated)
            {
                if (config.Translation1 != -1)
                {
                    // TODO: OutOfRange problem
                    string id = LoadedData.TranslationsList[config.Translation1].Identifier;
                    Translation1TB.Text = versions[id].Verses[verNum];
                    Translation1TB.Visibility = Visibility.Visible;
                }

                if (config.Translation2 != -1)
                {
                    string id = LoadedData.TranslationsList[config.Translation2].Identifier;
                    Translation2TB.Text = versions[id].Verses[verNum];
                    Translation2TB.Visibility = Visibility.Visible;
                }

                if (config.Translation3 != -1)
                {
                    string id = LoadedData.TranslationsList[config.Translation3].Identifier;
                    Translation3TB.Text = versions[id].Verses[verNum];
                    Translation3TB.Visibility = Visibility.Visible;
                }
            }

            // Measure and pre-arrange so that the size is known from the initialization
            Measure(new Size(650, double.PositiveInfinity));
            Arrange(new Rect(0, 0, 650, DesiredSize.Height));
        }
    }
}