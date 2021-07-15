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
    /// Logique d'interaction pour CheikhSelectorPage.xaml
    /// </summary>
    public partial class CheikhSelectorPage : Page
    {
        public bool ItemsInitialized { get; set; } = false;
        private BarakaPlayer _parentPlayer;

        public CheikhSelectorPage()
        {
            InitializeComponent();
        }

        public void InitializeItems(BarakaPlayer parentPlayer)
        {
            if (!ItemsInitialized)
            {
                for (int i = 0; i < LoadedData.CheikhList.Length; i++)
                {
                    var cheikh = LoadedData.CheikhList[i];

                    var card = new CheikhCard(cheikh, parentPlayer);
                    ContainerGrid.Children.Add(card);

                    if (i == LoadedData.Settings.DefaultCheikhIndex)
                    {
                        card.Select();
                        parentPlayer.ChangeSelectedCheikhCard(card);
                    }
                }

                PageSV.PreviewMouseWheel += parentPlayer.PageSV_PreviewMouseWheel;

                // Save parent player
                _parentPlayer = parentPlayer;

                ItemsInitialized = true;
            }
        }
    }
}
