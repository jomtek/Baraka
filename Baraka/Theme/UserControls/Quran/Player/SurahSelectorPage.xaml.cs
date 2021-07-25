using Baraka.Data;
using Baraka.Data.Descriptions;
using Baraka.Utils.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Baraka.Theme.UserControls.Quran.Player
{
    /// <summary>
    /// Logique d'interaction pour SurahSelectorPage.xaml
    /// </summary>
    public partial class SurahSelectorPage : Page
    {
        public bool ItemsInitialized { get; set; } = false;
        private BarakaPlayer _parentPlayer;

        #region Events
        [Category("Baraka")]
        public event EventHandler<VerseDescription> HizbClicked;
        #endregion

        public SurahSelectorPage()
        {
            InitializeComponent();
        }

        public void InitializeItems(BarakaPlayer parentPlayer)
        {
            if (!ItemsInitialized)
            {
                PageSV.PreviewMouseWheel += parentPlayer.PageSV_PreviewMouseWheel;
                
                // Save parent player
                _parentPlayer = parentPlayer;

                ItemsInitialized = true;
            }

            foreach (SurahDescription surah in LoadedData.SurahList.Keys)
            {
                var bar = new SurahBar(surah, parentPlayer);
                ContainerSP.Children.Add(bar);
            }
        }

        public void RefreshSelection()
        {
            if (!ItemsInitialized)
            {
                return;
            }

            if (_parentPlayer.SelectedSurah == null)
            {
                var firstBar = (SurahBar)ContainerSP.Children[0];
                _parentPlayer.SetSelectedBar(firstBar);
                firstBar.Select();
            }
            else
            {
                foreach (SurahBar bar in ContainerSP.Children)
                {
                    if (bar.Surah.PhoneticName == _parentPlayer.SelectedSurah.PhoneticName)
                    {
                        _parentPlayer.SetSelectedBar(bar);
                        bar.Select();

                        // Auto-scroll to bar
                        int index = ContainerSP.Children.IndexOf(bar);
                        if (index > 6)
                        {
                            PageSV.ScrollToVerticalOffset(bar.ActualHeight * (index - 3));
                        }
                        else
                        {
                            PageSV.ScrollToVerticalOffset(0);
                        }
                    }
                    else
                    {
                        bar.Unselect();
                    }
                }
            }
        }

        #region Juz and Hizb
        public void HizbSelected(VerseDescription start)
        {
            HizbClicked?.Invoke(this, start);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Load hizb visualizer
            HizbVisualizer.Page = this;
            HizbVisualizer.LoadSegments();

            // TODO
            //PageSV.Focus();
        }
        #endregion

        #region Search
        private async Task SendQuery()
        {
            var query = General.PrepareQuery(SearchTB.Text);

            if (query.Length > 0 && query != SearchTB.Placeholder)
            {
                await ProcessQuery(query);
            }
            else
            {
                if (ContainerSP.Children.Count < 114)
                {
                    // Re-load Surah list
                    InitializeItems(null);
                }
            }
        }

        private async void SearchTB_TextChanged(object sender, EventArgs e)
        {
            if (ItemsInitialized)
            {
                await SendQuery();
            }
        }

        private async Task ProcessQuery(string query, bool showAll = false)
        {
            await Task.Delay(300);

            if (query != General.PrepareQuery(SearchTB.Text))
            {
                return;
            }

            ContainerSP.Children.Clear();


            Console.WriteLine($"test: {query}");
        }
        #endregion
    }
}
