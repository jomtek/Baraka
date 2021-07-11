using Baraka.Data;
using Baraka.Data.Descriptions;
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

namespace Baraka.Theme.UserControls.Quran.Player
{
    /// <summary>
    /// Logique d'interaction pour SurahSelectorPage.xaml
    /// </summary>
    public partial class SurahSelectorPage : Page
    {
        public bool ItemsInitialized { get; set; } = false;
        private BarakaPlayer _parentPlayer;

        public SurahSelectorPage()
        {
            InitializeComponent();
        }

        public void InitializeItems(BarakaPlayer parentPlayer)
        {
            if (!ItemsInitialized)
            {
                foreach (SurahDescription surah in LoadedData.SurahList.Keys)
                {
                    var bar = new SurahBar(surah, parentPlayer);
                    ContainerSP.Children.Add(bar);
                }

                // Save parent player
                _parentPlayer = parentPlayer;

                ItemsInitialized = true;
            }
        }

        public void RefreshSelection(SurahDescription selectedSurah)
        {
            if (selectedSurah == null)
            {
                var firstBar = (SurahBar)ContainerSP.Children[0];
                _parentPlayer.SetSelectedBar(firstBar);
                firstBar.Select();
            }
            else
            {
                foreach (SurahBar bar in ContainerSP.Children)
                {
                    if (bar.Surah.PhoneticName == selectedSurah.PhoneticName)
                    {
                        _parentPlayer.SetSelectedBar(bar);
                        bar.Select();

                        // Auto-scroll to bar
                        int index = ContainerSP.Children.IndexOf(bar);
                        if (index > 6)
                        {
                            _parentPlayer.DisplaySV.ScrollToVerticalOffset(bar.ActualHeight * (index - 3));
                        }
                    }
                }
            }
        }
    }
}
