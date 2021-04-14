using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Baraka.Forms;
using Baraka.Utils.Search;

namespace Baraka.Theme.UserControls.Quran.Searcher
{
    /// <summary>
    /// Logique d'interaction pour BarakaSearchResult.xaml
    /// </summary>
    public partial class BarakaSearchResult : UserControl
    {
        private SearchResult _sres;
        private SearchWindow _window;

        public BarakaSearchResult(SearchResult sres, SearchWindow window)
        {
            InitializeComponent();

            _sres = sres;
            _window = window;

            // Load verse info
            VerseInfoTB.Text = $"S{sres.Surah.SurahNumber}. V{sres.Verse + 1}";

            // TODO
            string fullVerse = Data.LoadedData.SurahList.ElementAt(sres.Surah.SurahNumber - 1).Value.ElementAt(2).Verses[sres.Verse];
            RTB.Document.Blocks.Add(new Paragraph(new Run(fullVerse)));

            HighlightTerms();
        }

        private string CreatePattern(string pattern)
        {
            return pattern.Replace("?", @"\?");
        }

        private void HighlightTerms()
        {
            foreach (string kw in _sres.Terms)
            {
                TextPointer pointer = RTB.Document.ContentStart;
                while (pointer != null)
                {
                    if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                    {
                        string textRun = pointer.GetTextInRun(LogicalDirection.Forward);

                        MatchCollection matches = Regex.Matches(
                            textRun,
                            CreatePattern(kw),
                            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
                        );

                        foreach (Match match in matches)
                        {
                            TextPointer start = pointer.GetPositionAtOffset(match.Index);
                            TextPointer end = start.GetPositionAtOffset(match.Length);

                            var selection = new TextRange(start, end);

                            if (selection.Text.ToLower() != kw)
                            {
                                continue;
                            }

                            selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                        }
                    }

                    pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
                }
            }
        }

        private void RTB_SelectionChanged(object sender, RoutedEventArgs e)
        {
        
        }

        private void VerseInfoTB_MouseEnter(object sender, MouseEventArgs e)
        {
            VerseInfoTB.Background = new SolidColorBrush(SystemColors.ControlLightColor);
        }

        private void VerseInfoTB_MouseLeave(object sender, MouseEventArgs e)
        {
            VerseInfoTB.Background = Brushes.Transparent;
        }

        private void Viewbox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _window.ResultClicked(_sres);
        }
    }
}
