using Baraka.Data;
using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.ComponentModel;

namespace Baraka.Theme.UserControls.Quran.Display.Translated
{
    /// <summary>
    /// Logique d'interaction pour BarakaVerse.xaml
    /// </summary>
    public partial class BarakaVerse : UserControl
    {
        public SurahDescription Surah { get; private set; }
        public int Number { get; private set; }

        private List<Run> _arabicInlines;
        private bool _loadLastBookmark;

        #region Events
        [Category("Baraka")]
        public event EventHandler<bool> CompletedInitialize;
        #endregion

        public BarakaVerse(SurahDescription surah, int number,
            bool loadLastBookmark = false) // See BarakaTranslatedSurahDisplayer.cs for explanations about this
        {
            InitializeComponent();

            DataContext = this;

            Surah = surah;
            Number = number;

            _arabicInlines = new List<Run>();
            _loadLastBookmark = loadLastBookmark;
        }

        public void LoadArabicVersion(FontFamily pageFamily, string glyphs, int page)
        {
            ArabicTB.FontFamily = pageFamily;

            foreach (char glyph in glyphs) // Works the same way as the Mushaf (one glyph = one word with p.s. font)
            {
                var run = new Run(glyph.ToString());
                ArabicTB.Inlines.Add(run);

                // TODO: do not let ayah number sit on a line by itself


                // Do not add stopping signs to the inlines list
                double measure = Utils.General.MeasureText(glyph.ToString(), ArabicTB);
                if (measure > 10)
                {
                    _arabicInlines.Add(run);
                }
                else
                {
                    run.Foreground = Brushes.DeepSkyBlue;
                }
            }

            if (page == 1 || page == 2)
            {
                ArabicTB.FontSize *= 4 / 3d; // quran.com
            }
        }

        public void Initialize()
        {
            int verNum = Number;

            Dictionary<string, SurahVersion> versions = Data.LoadedData.SurahList[Surah];
            var config = Data.LoadedData.Settings.SurahVersionConfig;

            // For performance reasons, the arabic version is loaded from the Displayer directly         
            if (config.DisplayArabic)
            {
                ArabicTB.Visibility = Visibility.Visible;
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
                    // TODO: manage OutOfRange problem
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
        }

        #region Karaoke      
        public void HighlightWord(int index)
        {
            if (ArabicTB.Visibility == Visibility.Collapsed)
            {
                return;
            }

            ClearHighlighting();

            Run inline = _arabicInlines[index];
            inline.Background = Brushes.LightGoldenrodYellow;
        }

        public void ClearHighlighting()
        {
            foreach (Run inline in _arabicInlines)
            {
                inline.Background = Brushes.Transparent;
            }
        }
        #endregion

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CompletedInitialize?.Invoke(this, _loadLastBookmark);
        }
    }
}