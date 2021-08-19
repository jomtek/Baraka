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
using System.ComponentModel;
using System.Data.SQLite;
using System.Data;
using Dapper;
using Baraka.Data.Descriptions;
using Baraka.Data;
using System.Net;
using System.IO;

namespace Baraka.Theme.UserControls.Quran.Display.Mushaf
{
    public enum MadinaPageSide
    {
        LEFT,
        RIGHT
    }

    /// <summary>
    /// Logique d'interaction pour BarakaMadinaPage.xaml
    /// </summary>
    public partial class BarakaMadinaPage : UserControl, INotifyPropertyChanged
    {
        private MadinaPageSide _side;

        #region PropertyChanged Notifier
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Settings
        private double _cornerIntensity = 25;

        [Category("Custom")]
        public MadinaPageSide Side
        {
            get { return _side; }
            set
            {
                if (value == MadinaPageSide.LEFT)
                {
                    BorderComponent.CornerRadius = new CornerRadius(_cornerIntensity, 0, 0, _cornerIntensity);
                }
                else if (value == MadinaPageSide.RIGHT)
                {
                    BorderComponent.CornerRadius = new CornerRadius(0, _cornerIntensity, _cornerIntensity, 0);
                }
                _side = value;

                RaisePropertyChanged("Side");
            }
        }
        #endregion

        public BarakaMadinaPage()
        {
            InitializeComponent();
            //LoadPage(53);
        }

        #region Utils
        private int GetReferenceAyahWordCount(VerseDescription verse)
        {
            return verse.ArabicText.Split(' ').Length;
        }
        #endregion

        #region Core
        public void LoadPage(int pageIdx) // pageIdx starts at 0
        {
            /*
            // Clear page text
            PageTB.Text = "";

            // Prepare page-specific font family
            PageTB.FontFamily = LoadedData.MushafDataManager.FindPageFontFamily(pageIdx);

            // Load all data from madani_quran.db
            List<MadaniQueryRes> lines = LoadedData.MushafDataManager.RawLookup($"page={pageIdx + 1}");

            // Iterate over the entries
            //
            int totalWordsForCurrentAyah = 0;

            // We use a reference word count in order to identify the end-of-verse glyph 
            var verseDesc = new VerseDescription(Utils.Quran.General.FindSurah(lines[0].sura), 1);
            var referenceWordCount = GetReferenceAyahWordCount(verseDesc);

            for (int i = 0; i < lines.Count; i++)
            {
                MadaniQueryRes line = lines[i];

                // Skip if the line is irrelevant        
                if (line.ayah == 0)
                {
                    continue;
                }

                // Iterate through the glyphs (one glyph shows one word)
                string glyphs = WebUtility.HtmlDecode(line.text);
                for (int j = 0; j < glyphs.Length; j++)
                {
                    PageTB.Text += glyphs[j];
                    totalWordsForCurrentAyah++;

                    if (totalWordsForCurrentAyah == referenceWordCount)
                    {
                        totalWordsForCurrentAyah = 0;
                        var nextVerse = verseDesc.Next();
                        referenceWordCount = GetReferenceAyahWordCount(nextVerse);
                    }

                }
                PageTB.Text += "\n";
            }
            */
            System.Windows.Clipboard.SetText(PageTB.Text);
        }
        #endregion
    }
}
