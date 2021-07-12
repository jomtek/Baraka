using Baraka.Data;
using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using Baraka.Theme.UserControls.Quran.TranslationsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Baraka.Forms
{
    /// <summary>
    /// Logique d'interaction pour QuranEditionsManager.xaml
    /// </summary>
    public partial class QuranTranslationsManagerWindow : Window
    {
        private bool _changesMade = false;

        public QuranTranslationsManagerWindow()
        {
            InitializeComponent();
            LoadTranslations();
        }

        #region Load and save
        private void LoadTranslations()
        {
            // All translations
            for (int i = 0; i < LoadedData.TranslationsList.Length; i++)
            {
                var bar = new TranslationBar(LoadedData.TranslationsList[i], i);
                bar.PreviewMouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
                {
                    _changesMade = true;

                    if (bar.Selected)
                    {
                        SelectedTranslationsSP.Children.Remove(bar);
                        AllTranslationsSP.Children.Insert(bar.Index, bar);
                    }
                    else
                    {
                        if (SelectedTranslationsSP.Children.Count < 3)
                        {
                            AllTranslationsSP.Children.Remove(bar);
                            SelectedTranslationsSP.Children.Add(bar);
                            SelectedTranslationsSV.ScrollToEnd();
                        }
                        else
                        {
                            bar.Selected = !bar.Selected; // Forsee incoming line
                            SystemSounds.Beep.Play();
                        }
                    }
                    
                    bar.Selected = !bar.Selected;
                };

                AllTranslationsSP.Children.Add(bar);
            }


            // Selected translations
            foreach (TranslationDescription translation in new[] {
                LoadedData.Settings.SurahVersionConfig.Translation1,
                LoadedData.Settings.SurahVersionConfig.Translation2,
                LoadedData.Settings.SurahVersionConfig.Translation3
            })
            {
                if (translation != null)
                {
                    int idx = Array.IndexOf(LoadedData.TranslationsList, translation);
                    
                    TranslationBar bar = (TranslationBar)AllTranslationsSP.Children[idx];
                    bar.Selected = true;

                    AllTranslationsSP.Children.RemoveAt(idx);
                    SelectedTranslationsSP.Children.Add(bar);
                }
            }
        }

        #endregion

        private bool TranslationExists(string identifier)
        {
            // Check with Al-Fatiha
            return LoadedData.SurahList.ElementAt(0).Value.ContainsKey(identifier);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_changesMade)
                return;

            var sb = new StringBuilder("Voulez-vous sauvegarder les changements ?");
            sb.AppendLine();
            sb.AppendLine(@"Des téléchargements risquent d'être effectués.");

            var result = MessageBox.Show(sb.ToString(), "Baraka - Panel de traductions", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (SelectedTranslationsSP.Children.Count > 0)
                    {
                        LoadedData.Settings.SurahVersionConfig.Translation1 = null;
                        LoadedData.Settings.SurahVersionConfig.Translation2 = null;
                        LoadedData.Settings.SurahVersionConfig.Translation3 = null;

                        for (int i = 0; i < SelectedTranslationsSP.Children.Count; i++)
                        {
                            var bar = (TranslationBar)SelectedTranslationsSP.Children[i];

                            // Download translations
                            if (!TranslationExists(bar.Description.Identifier))
                            {
                                using (var wc = new WebClient()
                                {
                                    Encoding = Encoding.GetEncoding("UTF-8")
                                })
                                {
                                    string rawData;

                                    try
                                    {
                                        using (new Utils.WaitCursor())
                                        {
                                            // TODO: adapt to private server
                                            rawData = wc.DownloadString($"https://tanzil.net/trans/{bar.Description.Identifier}");
                                        }
                                    }
                                    catch (WebException ex)
                                    {
                                        Utils.Emergency.ShowExMessage(ex);
                                        MessageBox.Show("Les changements ont été abandonnés.", "Baraka - Panel de traductions");
                                        return;
                                    }

                                    var parsed = new TanzilTranslationParser(rawData).Result;
                                    for (int j = 0; j < parsed.Count; j++)
                                    {
                                        string[] verses = parsed[j];
                                        LoadedData.SurahList.ElementAt(j).Value.Add(
                                            bar.Description.Identifier,
                                            new SurahVersion(bar.Description.LanguageName_EN, verses)
                                        );
                                    }
                                }
                            }

                            // Save selected translations
                            switch (i)
                            {
                                case 0:
                                    LoadedData.Settings.SurahVersionConfig.Translation1 = bar.Description;
                                    break;
                                case 1:
                                    LoadedData.Settings.SurahVersionConfig.Translation2 = bar.Description;
                                    break;
                                case 2:
                                    LoadedData.Settings.SurahVersionConfig.Translation3 = bar.Description;
                                    break;
                            }

                        }

                        SerializationUtils.Serialize(LoadedData.SurahList, "data/quran.ser");
                    }
                    else
                    {
                        LoadedData.Settings.SurahVersionConfig.Translation1 = null;
                        LoadedData.Settings.SurahVersionConfig.Translation2 = null;
                        LoadedData.Settings.SurahVersionConfig.Translation3 = null;
                    }

                    MessageBox.Show(
                        "Les changements ont été effectués avec succès et prendront effet au prochain lancement du logiciel.",
                        "Baraka - Panel de traductions",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    break;

                case MessageBoxResult.No:
                    break;
                
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }
    }
}
