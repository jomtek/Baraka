using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
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

namespace Baraka.Theme.UserControls.Quran.Displayer
{
    /// <summary>
    /// Logique d'interaction pour BarakaVerse.xaml
    /// </summary>
    public partial class BarakaVerse : UserControl
    {
        private SurahDescription _surah;
        private int _number;

        public BarakaVerse(SurahDescription surah, int number)
        {
            InitializeComponent();
        
            _surah = surah;
            _number = number;
        }

        public void Initialize()
        {
            SurahVersion[] versions = Data.LoadedData.SurahList[_surah];
            ArabicTB.Text = versions[0].Verses[_number];
            PhoneticTB.Text = versions[1].Verses[_number];
            TranslatedTB.Text = versions[2].Verses[_number];
        }
    }
}