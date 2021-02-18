using Baraka.Data;
using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
using System.IO;
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

// todo remove
using System.Xml;
using System.Xml.Serialization;

namespace Baraka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.GetPosition(this).Y < TopGrid.Height)
            {
                DragMove();
            }
        }

        private void CloseFormCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
            CloseFormPath.Fill = new SolidColorBrush(Color.FromRgb(222, 222, 222));
        }

        private void CloseFormCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            CloseFormPath.Fill = Brushes.White;
        }

        private void CloseFormCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            /*string translationPath = @"C:\Users\jomtek360\Documents\Baraka\quran-translated-main";
            foreach (var desc in SurahList)
            {
                var surahNum = desc.SurahNumber;
                var arVersion = File.ReadAllLines($@"{translationPath}\arabic_uthmani\{surahNum}");
                var phVersion = File.ReadAllLines($@"{translationPath}\phonetic\{surahNum}");
                var frVersion = File.ReadAllLines($@"{translationPath}\french_hamidullah\{surahNum}");

                SerializationData.Add(desc, new Data.Surah.SurahVersion[]
                {
                    new Data.Surah.SurahVersion("arabic", "uthmani", arVersion),
                    new Data.Surah.SurahVersion("phonetic", "", phVersion),
                    new Data.Surah.SurahVersion("french", "hamidullah", frVersion),
                });
            }
            Data.SerializationUtils.Serialize(SerializationData, "quran.ser");*/

            LoadedData.SurahList = SerializationUtils.Deserialize<Dictionary<SurahDescription, Data.Surah.SurahVersion[]>>("quran.ser");
        }
    }
}