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

namespace Baraka.Theme.UserControls.Quran.Player.Selectors.Surah
{
    /// <summary>
    /// Logique d'interaction pour BarakaHizbSegment.xaml
    /// </summary>
    public partial class BarakaHizbSegment : UserControl
    {
        private bool _selected = false;

        #region Settings
        [Category("Baraka")]
        public VerseDescription Limit { get; set; }

        [Category("Baraka")]
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                
                if (value)
                {
                    Console.WriteLine("yes");
                    BorderBrush = Brushes.Goldenrod;
                }
                else
                {
                    BorderBrush = Brushes.Transparent;
                }
            }
        }
        #endregion

        public BarakaHizbSegment()
        {
            InitializeComponent();
        }

        #region Interaction
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            BorderBrush = Brushes.Goldenrod;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!Selected)
            {
                BorderBrush = Brushes.Transparent;
            }
        }
        #endregion
    }
}
