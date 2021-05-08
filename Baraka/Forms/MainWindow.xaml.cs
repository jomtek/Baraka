﻿using Baraka.Data.Descriptions;
using Baraka.Forms;
using Baraka.Forms.Settings;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

// todo remove

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

            ((Storyboard)this.Resources["DashboardCloseStory"]).Begin();
            ((Storyboard)this.Resources["DashboardCloseStory"]).SkipToFill();

            // Bind the displayer to the player
            Player.Displayer = MainSurahDisplayer;
        }

        private Dictionary<SurahDescription, Data.Surah.SurahVersion[]> SerializationData =
            new Dictionary<SurahDescription, Data.Surah.SurahVersion[]>();

        private void Window_Initialized(object sender, EventArgs e)
        {
            /*string translationPath = @"C:\Users\jomtek360\Documents\Baraka\quran-translated-main";
            foreach (var elem in LoadedData.SurahList)
            {
                var desc = elem.Key;
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
            Data.SerializationUtils.Serialize(SerializationData, "qurannew.ser");*/

            /*
            var tempCheikhs = new CheikhDescription[]
            {
                new CheikhDescription("Mishary bin Rashid", "Alafasy", "https://everyayah.com/data/Alafasy_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/alafasy.png"))),
                new CheikhDescription("Abderrahman", "Al-Sudais", "https://everyayah.com/data/Abdurrahmaan_As-Sudais_192kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/soudais.png"))),
                new CheikhDescription("Maher", "Al-Mueaqly", "https://everyayah.com/data/MaherAlMuaiqly128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/maher.png"))),
                new CheikhDescription("Saad", "Al-Ghamidi", "https://everyayah.com/data/Ghamadi_40kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/ghamidi.png"))),
                new CheikhDescription("Saleh", "Bukhatir", "https://everyayah.com/data/Salaah_AbdulRahman_Bukhatir_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/bukhatir.png"))),
                new CheikhDescription("Yasser", "Al-Dosari", "https://everyayah.com/data/Yasser_Ad-Dussary_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/dosari.png"))),
                new CheikhDescription("Saoud", "Shuraim", "https://everyayah.com/data/Saood_ash-Shuraym_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/shuraim.png"))),
                new CheikhDescription("Sahl", "Yassin", "https://everyayah.com/data/Sahl_Yassin_128kbps", new BitmapImage(new Uri(@"pack://application:,,,/Baraka;component/Images/Cheikh/yassin.png"))),
            };

            Data.SerializationUtils.Serialize(tempCheikhs, "cheikh.ser");*/
        }

        #region Window events
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Player.Dispose();
            Environment.Exit(0);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
        #endregion

        #region Displayer to Player events
        private void MainSurahDisplayer_VerseChanged(object sender, int num)
        {
            Console.WriteLine($"verse changed to : {num}");
            Player.ChangeVerse(num);
        }

        private void MainSurahDisplayer_DownloadVerseRequested(object sender, int num)
        {
            Player.DownloadMp3Verse(num);
        }
        #endregion

        #region Dashboard
        private bool _dashboardOpened = false;

        #region Animation
        private void Dashboard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!_dashboardOpened)
            {
                ((Storyboard)this.Resources["DashboardOpenStory"]).Begin();
                _dashboardOpened = true;
            }
        }

        private void Dashboard_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_dashboardOpened)
            {
                ((Storyboard)this.Resources["DashboardCloseStory"]).Begin();
                _dashboardOpened = false;
            }
        }
        #endregion
        #region Items
        private void SearchDashboardItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowBlurEffect.Radius = 8;

            var myInstance = new SearchWindow() { Owner = this };
            myInstance.VerseClicked += (object _, VerseDescription verse) =>
            {
                Player.ChangeSelectedSurah(verse.Surah);
                Player.ChangeVerse(verse.Number);
                MainSurahDisplayer.ScrollToVerse(verse.Number, true);
                Player.Playing = false;
            };
            myInstance.ShowDialog();

            WindowBlurEffect.Radius = 0;
        }

        private void SettingsDashboardItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowBlurEffect.Radius = 8;
            new SettingsWindow() { Owner = this }.ShowDialog();
            WindowBlurEffect.Radius = 0;
        }
        #endregion
        #endregion
    }
}