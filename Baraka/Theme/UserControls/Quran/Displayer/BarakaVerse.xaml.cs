using Baraka.Data;
using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Baraka.Theme.UserControls.Quran.Displayer
{
    /// <summary>
    /// Logique d'interaction pour BarakaVerse.xaml
    /// </summary>
    public partial class BarakaVerse : UserControl
    {
        public SurahDescription Surah { get; private set; }
        public int Number { get; private set; }

        private List<Run> _arabicInlines;

        public BarakaVerse(SurahDescription surah, int number)
        {
            InitializeComponent();

            DataContext = this;

            Surah = surah;
            Number = number;

            _arabicInlines = new List<Run>();
        }

        public void Initialize()
        {
            int verNum = Number;

            Dictionary<string, SurahVersion> versions = Data.LoadedData.SurahList[Surah];
            var config = Data.LoadedData.Settings.SurahVersionConfig;
            
            if (config.DisplayArabic)
            {
                string[] words = versions["ARABIC"].Verses[verNum].Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    var run = new Run(words[i]); // A run associated with the current word

                    _arabicInlines.Add(run);
                    ArabicTB.Inlines.Add(run); 

                    if (i != words.Length)
                    {
                        ArabicTB.Inlines.Add(new Run("  "));
                    }
                    else
                    {
                        run.Background = Brushes.LightCyan;
                    }
                }

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
            Measure(new System.Windows.Size(650, double.PositiveInfinity));
            Arrange(new Rect(0, 0, 650, DesiredSize.Height));

            //HighlightWord(0);
        }

        #region Karaoke      
        public void HighlightWord(int index)
        {
            CleanHighlighting();

            try
            {
                Run inline = _arabicInlines[index];
                inline.Background = Brushes.YellowGreen;
            } catch
            {

            }
        }

        public void CleanHighlighting()
        {
            foreach (Run inline in _arabicInlines)
            {
                inline.Background = Brushes.Transparent;
            }
        }
        #endregion
    }
}