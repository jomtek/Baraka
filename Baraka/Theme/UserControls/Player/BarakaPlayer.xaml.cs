using Baraka.Data;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Baraka.Data.Descriptions;

namespace Baraka.Theme.UserControls.Player
{
    /// <summary>
    /// Logique d'interaction pour BarakaPlayer.xaml
    /// </summary>
    public partial class BarakaPlayer : UserControl
    {
        private bool _playing = false;
        private bool _loopMode = false;
        private bool _cheikhModification = false;
        private bool _surahModification = false;
        private int _lastTabShown = -1;

        private CheikhDescription _selectedCheikh;
        private CheikhCard _selectedCheikhCard;

        private SurahDescription _selectedSurah;
        private SurahBar _selectedSurahBar;

        #region Settings
        public bool Playing
        {
            get { return _playing; }
            set
            {
                _playing = value;
                // todo
            }
        }
        public CheikhDescription SelectedCheikh
        {
            get { return _selectedCheikh; }
        }
        #endregion

        public BarakaPlayer()
        {
            InitializeComponent();
        }

        #region Controller Controls
        private void LoopBTN_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_loopMode)
            {
                LoopBTN.Fill = new ImageBrush(
                    new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/player_loop_off.png"))
                );
            }
            else
            {
                LoopBTN.Fill = new ImageBrush(
                    new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/player_loop_on.png"))
                );
            }

            _loopMode = !_loopMode;
        }

        private void CheikhTB_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_cheikhModification)
            {
                CheikhTB.Foreground = Brushes.Black;
                ClosePlayer();
            }
            else
            {
                if (_surahModification)
                {
                    SurahTB.Foreground = Brushes.Black;
                    _surahModification = false;
                    SwitchTab(0);
                }
                else
                {
                    OpenPlayer(0);
                }

                CheikhTB.Foreground = Brushes.Gray;
            }

            _cheikhModification = !_cheikhModification;
        }

        private void SurahTB_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_surahModification)
            {
                SurahTB.Foreground = Brushes.Black;
                ClosePlayer();
            }
            else
            {
                if (_cheikhModification)
                {
                    CheikhTB.Foreground = Brushes.Black;
                    _cheikhModification = false;
                    SwitchTab(1);
                }
                else
                {
                    OpenPlayer(1);
                }

                SurahTB.Foreground = Brushes.Gray;
            }

            _surahModification = !_surahModification;
        }

        private void PlayPauseBTN_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_playing)
            {
                PlayPauseBtnPath.Style = (Style)this.Resources["Play"];
            }
            else
            {
                PlayPauseBtnPath.Style = (Style)this.Resources["Pause"];
            }

            _playing = !_playing;
        }
        #endregion

        #region Scrollbar
        private void MainSB_OnScroll(object sender, EventArgs e)
        {
            Console.WriteLine($"on scroll: scrolled: {MainSB.Scrolled}");
            DisplaySV.ScrollToVerticalOffset(DisplaySV.ScrollableHeight * MainSB.Scrolled);
        }
        #endregion

        #region Scrollviewer Display
        private void OpenPlayer(int tab)
        {
            ((Storyboard)this.Resources["PlayerOpenStory"]).Begin();
            SwitchTab(tab, false);
        }

        private void ClosePlayer()
        {
            ((Storyboard)this.Resources["PlayerCloseStory"]).Begin();
            // todo: dispose
        }

        private void SwitchTab(int tab, bool animation = true)
        {
            DisplaySV.ScrollToTop();
            MainSB.Reset();

            if (animation) ((Storyboard)this.Resources["TabSwitchStory"]).Begin();
            if (_lastTabShown == tab) return;

            switch (tab)
            {
                case 0:
                    ShowCheikhSelector();
                    break;
                case 1:
                    ShowSurahSelector();
                    break;
                default: throw new NotImplementedException();
            }

            _lastTabShown = tab;
        }

        #region Cheikh Selector
        private void ShowCheikhSelector()
        {
            var tempCheikhs = new CheikhDescription[]
            {
                new CheikhDescription("Mishary bin Rashid", "Alafasy", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/alafasy.png"))),
                new CheikhDescription("Abderrahman", "Al-Sudais", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/soudais.png"))),
                new CheikhDescription("Maher", "Al-Mueaqly", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/maher.png"))),
                new CheikhDescription("Yasser", "Al-Dosari", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/dosari.png"))),
                new CheikhDescription("Abdelbasset", "Abdessamad", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/abdessamad.png"))),
            };

            MainSB.TargetValue = (int)Math.Ceiling(tempCheikhs.Length / 3d); // todo: change

            var panel = new StackPanel();
            panel.HorizontalAlignment = HorizontalAlignment.Stretch;
            panel.VerticalAlignment = VerticalAlignment.Stretch;

            // // Cheikh repartition
            var cheikhPairs = new List<(CheikhDescription, CheikhDescription, CheikhDescription)>();
            
            var lastThreeCheikhs = new List<CheikhDescription>();
            foreach (var cheikh in tempCheikhs)
            {
                if (lastThreeCheikhs.Count < 3)
                {
                    lastThreeCheikhs.Add(cheikh);
                }
                else
                {
                    var pair = (lastThreeCheikhs[0], lastThreeCheikhs[1], lastThreeCheikhs[2]);
                    cheikhPairs.Add(pair);

                    lastThreeCheikhs.Clear();
                    lastThreeCheikhs.Add(cheikh);
                }
            }

            // Manage remaining pair
            var remainingPair = new CheikhDescription[3] { null, null, null };
            if (lastThreeCheikhs.ElementAtOrDefault(0) != null) remainingPair[0] = lastThreeCheikhs[0];
            if (lastThreeCheikhs.ElementAtOrDefault(1) != null) remainingPair[1] = lastThreeCheikhs[1];
            if (lastThreeCheikhs.ElementAtOrDefault(2) != null) remainingPair[2] = lastThreeCheikhs[2];
            cheikhPairs.Add((remainingPair[0], remainingPair[1], remainingPair[2]));

            // Create sub-panels
            foreach (var idenPair in cheikhPairs)
            {
                var subPanel = new StackPanel();
                subPanel.Orientation = Orientation.Horizontal;
                
                // Prevent shadows from getting clipped
                subPanel.Height = 215;
                subPanel.Margin = new Thickness(5, 2, 0, 0);
                // //

                var card1 = new CheikhCard(idenPair.Item1, this)
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(20, 0, 15, 0)
                };
                subPanel.Children.Add(card1);

                if (idenPair.Item2 != null)
                {
                    var card2 = new CheikhCard(idenPair.Item2, this)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    subPanel.Children.Add(card2);
                }

                if (idenPair.Item3 != null)
                {
                    var card3 = new CheikhCard(idenPair.Item3, this)
                    {
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Margin = new Thickness(15, 0, 20, 0)
                    };
                    subPanel.Children.Add(card3);
                }

                if (_selectedCheikh != null)
                {
                    foreach (CheikhCard card in subPanel.Children)
                    {
                        if (card.Cheikh.ToString() == _selectedCheikh.ToString())
                        {
                            ChangeSelectedCheikh(card);
                            card.Select();
                        }
                    }
                }

                panel.Children.Add(subPanel);
            }

            DisplaySV.Content = panel;
        }

        public void ChangeSelectedCheikh(CheikhCard card)
        {
            if (card != _selectedCheikhCard)
            {
                _selectedCheikh = card.Cheikh;
                if (_selectedCheikhCard != null)
                    _selectedCheikhCard.Unselect();
                _selectedCheikhCard = card;

                CheikhTB.Text = _selectedCheikh.ToString();
            }
        }

        public void ChangeSelectedSurah(SurahBar bar)
        {
            if (bar != _selectedSurahBar)
            {
                _selectedSurah = bar.Surah;
                if (_selectedSurahBar != null)
                    _selectedSurahBar.Unselect();
                _selectedSurahBar = bar;

                SurahTB.Text = _selectedSurah.PhoneticName;
            }
        }
        #endregion

        #region Surah Selector
        private void ShowSurahSelector()
        {
            MainSB.TargetValue = (int)Math.Ceiling(114 / 3d); // todo: change

            var panel = new StackPanel();
            panel.HorizontalAlignment = HorizontalAlignment.Stretch;
            panel.VerticalAlignment = VerticalAlignment.Stretch;
            
            foreach (var surahDesc in LoadedData.SurahList)
            {
                var bar = new SurahBar(surahDesc.Key, this);

                if (_selectedSurah != null)
                {
                    if (bar.Surah.PhoneticName == _selectedSurah.PhoneticName)
                    {
                        ChangeSelectedSurah(bar);
                        bar.Select();
                    }
                }

                panel.Children.Add(bar);
            }

            DisplaySV.Content = panel;
        }
        #endregion

        #endregion
    }
}