﻿using Baraka.Data;
using Baraka.Data.Descriptions;
using NAudio.Wave;
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

namespace Baraka.Forms.Settings
{
    /// <summary>
    /// Logique d'interaction pour ReadingPage.xaml
    /// </summary>
    public partial class ReadingPage : Page
    {
        public ReadingPage()
        {
            InitializeComponent();

            LoadSettings();

            // Fill Comboboxes
            foreach (SurahDescription surah in Data.LoadedData.SurahList.Keys)
            {
                DefaultSurahCMBB.Items.Add($"{surah.SurahNumber}. {surah.PhoneticName}");
            }
            foreach (CheikhDescription cheikh in Data.LoadedData.CheikhList)
            {
                DefaultCheikhCMBB.Items.Add(cheikh.ToString());
            }
            LoadOutputDevices();
        }

        #region Load and save
        private void LoadOutputDevices()
        {
            for (int i = 0; i < DirectSoundOut.Devices.Count(); i++)
            {
                DirectSoundDeviceInfo dev = DirectSoundOut.Devices.ElementAt(i);

                OutputDeviceCMBB.Items.Add(dev.Description);

                if (dev.Guid.ToString() == LoadedData.Settings.OutputDeviceGuid)
                {
                    OutputDeviceCMBB.SelectedIndex = i;
                }
            }

            // TODO: temporary
            if (OutputDeviceCMBB.SelectedIndex == -1)
            {
                LoadedData.Settings.OutputDeviceGuid = DirectSoundOut.Devices.ElementAt(0).Guid.ToString();
                OutputDeviceCMBB.SelectedIndex = 0;
            }
        }
        private void LoadSettings()
        {
            DefaultSurahCMBB.SelectedIndex = LoadedData.Settings.DefaultSurahIndex;
            DefaultCheikhCMBB.SelectedIndex = LoadedData.Settings.DefaultCheikhIndex;
            AutoScrollCHB.IsChecked = LoadedData.Settings.AutoScrollQuran;
            AutoNextCHB.IsChecked = LoadedData.Settings.AutoNextSurah;
            AutoReloadCHB.IsChecked = LoadedData.Settings.AutoReloadLastSurah;
            CrossFadingNUD.Value = LoadedData.Settings.CrossFadingValue;
            ArabicVersionCHB.IsChecked = LoadedData.Settings.SurahVersionConfig.DisplayArabic;
            PhoneticVersionCHB.IsChecked = LoadedData.Settings.SurahVersionConfig.DisplayPhonetic;
            TranslatedVersionCHB.IsChecked = LoadedData.Settings.SurahVersionConfig.DisplayTranslated;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            LoadedData.Settings.DefaultSurahIndex = DefaultSurahCMBB.SelectedIndex;
            LoadedData.Settings.DefaultCheikhIndex = DefaultCheikhCMBB.SelectedIndex;
            LoadedData.Settings.AutoScrollQuran = AutoScrollCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.AutoNextSurah = AutoNextCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.AutoReloadLastSurah = AutoReloadCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.CrossFadingValue = CrossFadingNUD.Value.GetValueOrDefault();
            LoadedData.Settings.OutputDeviceGuid = DirectSoundOut.Devices.ElementAt(OutputDeviceCMBB.SelectedIndex).Guid.ToString();
            LoadedData.Settings.SurahVersionConfig.DisplayArabic = ArabicVersionCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.SurahVersionConfig.DisplayPhonetic = PhoneticVersionCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.SurahVersionConfig.DisplayTranslated = TranslatedVersionCHB.IsChecked.GetValueOrDefault();
        }
        #endregion

        private void ManageEditionsBTN_Click(object sender, RoutedEventArgs e)
        {
            new QuranTranslationsManagerWindow() { Owner = (Window)Parent }.ShowDialog();
        }
    }
}
