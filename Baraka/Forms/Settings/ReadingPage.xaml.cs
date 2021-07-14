using Baraka.Data;
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
    public partial class ReadingPage : Page, ISettingsPage
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
            }

            OutputDeviceCMBB.SelectedIndex = LoadedData.Settings.OutputDeviceIndex;

            // TODO: temporary
            if (OutputDeviceCMBB.SelectedIndex == -1)
            {
                LoadedData.Settings.OutputDeviceIndex = 0;
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

        public void SaveSettings()
        {
            LoadedData.Settings.DefaultSurahIndex = DefaultSurahCMBB.SelectedIndex;
            LoadedData.Settings.DefaultCheikhIndex = DefaultCheikhCMBB.SelectedIndex;
            LoadedData.Settings.AutoScrollQuran = AutoScrollCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.AutoNextSurah = AutoNextCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.AutoReloadLastSurah = AutoReloadCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.CrossFadingValue = CrossFadingNUD.Value.GetValueOrDefault();
            LoadedData.Settings.OutputDeviceIndex = OutputDeviceCMBB.SelectedIndex;
            LoadedData.Settings.SurahVersionConfig.DisplayArabic = ArabicVersionCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.SurahVersionConfig.DisplayPhonetic = PhoneticVersionCHB.IsChecked.GetValueOrDefault();
            LoadedData.Settings.SurahVersionConfig.DisplayTranslated = TranslatedVersionCHB.IsChecked.GetValueOrDefault();
        }
        #endregion

        private void ManageEditionsBTN_Click(object sender, RoutedEventArgs e)
        {
            new QuranTranslationsManagerWindow() { Owner = (Window)Parent }.ShowDialog();
        }

        #region UI misc
        // Make it so that at least one language has to be selected 
        //
        private void ArabicVersionCHB_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!PhoneticVersionCHB.IsChecked.GetValueOrDefault() &&
                !TranslatedVersionCHB.IsChecked.GetValueOrDefault())
            {
                // Cancel the event
                ArabicVersionCHB.IsChecked = true;
            }
        }

        private void PhoneticVersionCHB_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!ArabicVersionCHB.IsChecked.GetValueOrDefault() &&
                !TranslatedVersionCHB.IsChecked.GetValueOrDefault())
            {
                // Cancel the event
                PhoneticVersionCHB.IsChecked = true;
            }
        }

        private void TranslatedVersionCHB_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!ArabicVersionCHB.IsChecked.GetValueOrDefault() &&
                !PhoneticVersionCHB.IsChecked.GetValueOrDefault())
            {
                // Cancel the event
                TranslatedVersionCHB.IsChecked = true;
            }
        }
        #endregion
    }
}
