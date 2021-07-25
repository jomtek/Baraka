using Baraka.Data;
using Baraka.Data.Descriptions;
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
                foreach (SurahDescription surah in LoadedData.SurahList.Keys)
                {
                    var bar = new SurahBar(surah, parentPlayer);
                    ContainerSP.Children.Add(bar);
                }
                
                PageSV.PreviewMouseWheel += parentPlayer.PageSV_PreviewMouseWheel;

                // Save parent player
                _parentPlayer = parentPlayer;

                ItemsInitialized = true;
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
        #endregion

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Load hizb visualizer
            HizbVisualizer.Page = this;
            HizbVisualizer.LoadSegments();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(((SurahBar)ContainerSP.Children[0]).ActualHeight);
            HizbVisualizer.LoadSegments();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var bg = HizbVisualizer.HizbSP;
        }
    }
}
