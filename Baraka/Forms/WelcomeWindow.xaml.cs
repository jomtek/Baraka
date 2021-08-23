using Baraka.Data;
using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Threading;
using Baraka.Data.Quran;
using Baraka.Theme.UserControls.Quran.Display;
using Baraka.Forms;
using Baraka.Data.Quran.Mushaf;
using Baraka.Theme.UserControls.Quran.Display.Mushaf;

namespace Baraka
{
    /// <summary>
    /// Logique d'interaction pour StartupForm.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        #region Serialization (debug purposes)
        private Dictionary<SurahDescription, Dictionary<string, SurahVersion>> SerializationData =
            new Dictionary<SurahDescription, Dictionary<string, SurahVersion>>();

        private void SerializeQuran()
        {
            SurahDescription[] SurahList = new SurahDescription[]
            {
                new SurahDescription(1,   7,    "Al-Fatiha",     "Prologue",                   SurahRevelationType.M),
                new SurahDescription(2,   286,  "Al-Baqarah",    "La vache",                   SurahRevelationType.H),
                new SurahDescription(3,   200,  "Al-Imran",      "La famille d'Imran",         SurahRevelationType.H),
                new SurahDescription(4,   176,  "An-Nisa",       "Les femmes",                 SurahRevelationType.H),
                new SurahDescription(5,   120,  "Al-Ma'ida",     "La table servie",            SurahRevelationType.H),
                new SurahDescription(6,   165,  "Al-Anam",       "Les bestiaux",               SurahRevelationType.M),
                new SurahDescription(7,   206,  "Al-Araf",       "Le purgatoire",              SurahRevelationType.M),
                new SurahDescription(8,   75,   "Al-Anfal",      "Le butin",                   SurahRevelationType.H),
                new SurahDescription(9,   129,  "At-Tawbah",     "Le repentir",                SurahRevelationType.H),
                new SurahDescription(10,  109,  "Yunus",         "Jonas",                      SurahRevelationType.M),
                new SurahDescription(11,  123,  "Hud",           "Hud",                        SurahRevelationType.M),
                new SurahDescription(12,  111,  "Yusuf",         "Joseph",                     SurahRevelationType.M),
                new SurahDescription(13,  43,   "Ar-Ra'd",       "Le tonnerre",                SurahRevelationType.M),
                new SurahDescription(14,  52,   "Ibrahim",       "Abraham",                    SurahRevelationType.M),
                new SurahDescription(15,  99,   "Al-Hijr",       "La vallée des pierres",      SurahRevelationType.M),
                new SurahDescription(16,  128,  "An-Nahl",       "Les abeilles",               SurahRevelationType.M),
                new SurahDescription(17,  111,  "Al-Isra",       "Le voyage nocturne",         SurahRevelationType.M),
                new SurahDescription(18,  110,  "Al-Kahf",       "La caverne",                 SurahRevelationType.M),
                new SurahDescription(19,  98,   "Maryam",        "Marie",                      SurahRevelationType.M),
                new SurahDescription(20,  135,  "Ta-Ha",         "Ta-Ha",                      SurahRevelationType.M),
                new SurahDescription(21,  112,  "Al-Anbiya",     "Les prophètes",              SurahRevelationType.M),
                new SurahDescription(22,  78,   "Al-Hajj",       "Le pélerinage",              SurahRevelationType.MH),
                new SurahDescription(23,  118,  "Al-Mu'minun",   "Les croyants",               SurahRevelationType.MH),
                new SurahDescription(24,  64,   "An-Nur",        "La lumière",                 SurahRevelationType.H),
                new SurahDescription(25,  77,   "Al-Furqan",     "Le discernement",            SurahRevelationType.MH),
                new SurahDescription(26,  227,  "Ash-Shu'ara",   "Les poètes",                 SurahRevelationType.M),
                new SurahDescription(27,  93,   "An-Naml",       "Les fourmis",                SurahRevelationType.MH),
                new SurahDescription(28,  88,   "Al-Qasas",      "Le récit",                   SurahRevelationType.M),
                new SurahDescription(29,  69,   "Al-Ankabut",    "L'araignée",                 SurahRevelationType.M),
                new SurahDescription(30,  60,   "Ar-Rum",        "Les romains",                SurahRevelationType.M),
                new SurahDescription(31,  34,   "Luqman",        "Luqman",                     SurahRevelationType.M),
                new SurahDescription(32,  30,   "As-Sajda",      "La prosternation",           SurahRevelationType.MH),
                new SurahDescription(33,  73,   "Al-Ahzab",      "Les coalisés",               SurahRevelationType.H),
                new SurahDescription(34,  54,   "Saba",          "Saba",                       SurahRevelationType.M),
                new SurahDescription(35,  45,   "Fatir",         "Le Créateur",                SurahRevelationType.M),
                new SurahDescription(36,  83,   "Ya-Sin",        "Ya-Sin",                     SurahRevelationType.M),
                new SurahDescription(37,  182,  "As-Saffat",     "Les rangés",                 SurahRevelationType.M),
                new SurahDescription(38,  88,   "Sad",           "Sad",                        SurahRevelationType.M),
                new SurahDescription(39,  75,   "Az-Zumar",      "Les groupes",                SurahRevelationType.M),
                new SurahDescription(40,  85,   "Al-Ghafir",     "Le pardonneur",              SurahRevelationType.M),
                new SurahDescription(41,  54,   "Fussilat",      "Les versets détaillés",      SurahRevelationType.M),
                new SurahDescription(42,  53,   "Ash-Shura",     "La consultation",            SurahRevelationType.M),
                new SurahDescription(43,  89,   "Az-Zukhruf",    "L'ornement",                 SurahRevelationType.MH),
                new SurahDescription(44,  59,   "Ad-Dukhan",     "La fumée",                   SurahRevelationType.M),
                new SurahDescription(45,  37,   "Al-Jathiya",    "L'agenouillée",              SurahRevelationType.M),
                new SurahDescription(46,  35,   "Al-Ahqaf",      "Les Dunes",                  SurahRevelationType.M),
                new SurahDescription(47,  38,   "Muhammad",      "Mohammed",                   SurahRevelationType.H),
                new SurahDescription(48,  29,   "Al-Fath",       "La victoire éclatante",      SurahRevelationType.H),
                new SurahDescription(49,  18,   "Al-Hujurat",    "Les appartements",           SurahRevelationType.H),
                new SurahDescription(50,  45,   "Qaf",           "Qaf",                        SurahRevelationType.M),
                new SurahDescription(51,  60,   "Ad-Dhariyat",   "Qui éparpillent",            SurahRevelationType.M),
                new SurahDescription(52,  49,   "At-Tur",        "La Montagne",                SurahRevelationType.M),
                new SurahDescription(53,  62,   "An-Najm",       "L'étoile",                   SurahRevelationType.M),
                new SurahDescription(54,  55,   "Al-Qamar",      "La lune",                    SurahRevelationType.M),
                new SurahDescription(55,  78,   "Ar-Rahman",     "Le Tout Miséricordieux",     SurahRevelationType.M),
                new SurahDescription(56,  96,   "Al-Waqi'a",     "L'événement",                SurahRevelationType.M),
                new SurahDescription(57,  29,   "Al-Hadid",      "Le fer",                     SurahRevelationType.H),
                new SurahDescription(58,  22,   "Al-Mujadila",   "La discussion",              SurahRevelationType.H),
                new SurahDescription(59,  24,   "Al-Hachr",      "L'exode",                    SurahRevelationType.H),
                new SurahDescription(60,  13,   "Al-Mumtahina",  "L'éprouvée",                 SurahRevelationType.H),
                new SurahDescription(61,  14,   "As-Saff",       "Le rang",                    SurahRevelationType.H),
                new SurahDescription(62,  11,   "Al-Jumua",      "Le vendredi",                SurahRevelationType.H),
                new SurahDescription(63,  11,   "Al-Munafiqun",  "Les hypocrites",             SurahRevelationType.H),
                new SurahDescription(64,  18,   "At-Taghabun",   "La grande perte",            SurahRevelationType.H),
                new SurahDescription(65,  12,   "At-Talaq",      "Le divorce",                 SurahRevelationType.H),
                new SurahDescription(66,  12,   "At-Tahrim",     "L'interdiction",             SurahRevelationType.H),
                new SurahDescription(67,  30,   "Al-Mulk",       "La royauté",                 SurahRevelationType.M),
                new SurahDescription(68,  52,   "Al-Qalam",      "La plume",                   SurahRevelationType.M),
                new SurahDescription(69,  52,   "Al-Haqqah",     "Celle qui montre la vérité", SurahRevelationType.M),
                new SurahDescription(70,  44,   "Al-Maarij",     "Les voies d'ascension",      SurahRevelationType.M),
                new SurahDescription(71,  28,   "Nuh",           "Noé",                        SurahRevelationType.M),
                new SurahDescription(72,  28,   "Al-Jinn",       "Les djinns",                 SurahRevelationType.M),
                new SurahDescription(73,  20,   "Al-Muzzamil",   "L'enveloppé",                SurahRevelationType.MH),
                new SurahDescription(74,  56,   "Al-Muddathir",  "Le revêtu d'un manteau",     SurahRevelationType.M),
                new SurahDescription(75,  40,   "Al-Qiyamah",    "La résurrection",            SurahRevelationType.M),
                new SurahDescription(76,  31,   "Al-Insan",      "L'Homme",                    SurahRevelationType.H),
                new SurahDescription(77,  50,   "Al-Mursalat",   "Les envoyés",                SurahRevelationType.MH),
                new SurahDescription(78,  40,   "An-Naba",       "La nouvelle",                SurahRevelationType.M),
                new SurahDescription(79,  46,   "An-Naziat",     "Les anges qui arrachent",    SurahRevelationType.M),
                new SurahDescription(80,  42,   "Abasa",         "Il s'est renfrogné",         SurahRevelationType.M),
                new SurahDescription(81,  29,   "At-Takwir",     "L'obscurcissement",          SurahRevelationType.M),
                new SurahDescription(82,  19,   "Al-Infitar",    "La rupture",                 SurahRevelationType.M),
                new SurahDescription(83,  36,   "Al-Mutaffifin", "Les fraudeurs",              SurahRevelationType.M),
                new SurahDescription(84,  25,   "Al-Insiqaq",    "La déchirure",               SurahRevelationType.M),
                new SurahDescription(85,  22,   "Al-Buruj",      "Les constellations",         SurahRevelationType.M),
                new SurahDescription(86,  17,   "Al-Tariq",      "L'astre nocturne",           SurahRevelationType.M),
                new SurahDescription(87,  19,   "Al-Ala",        "Le Très-Haut",               SurahRevelationType.M),
                new SurahDescription(88,  26,   "Al-Ghasiyah",   "L'enveloppante",             SurahRevelationType.M),
                new SurahDescription(89,  30,   "Al-Fajr",       "L'aube",                     SurahRevelationType.M),
                new SurahDescription(90,  20,   "Al-Balad",      "La cité",                    SurahRevelationType.M),
                new SurahDescription(91,  15,   "Ash-Shams",     "Le soleil",                  SurahRevelationType.M),
                new SurahDescription(92,  21,   "Al-Layl",       "La nuit",                    SurahRevelationType.M),
                new SurahDescription(93,  11,   "Ad-Duha",       "Le jour montant",            SurahRevelationType.M),
                new SurahDescription(94,  8,    "As-Sarh",       "L'ouverture",                SurahRevelationType.M),
                new SurahDescription(95,  8,    "At-Tin",        "Le figuier",                 SurahRevelationType.M),
                new SurahDescription(96,  19,   "Al-Alaq",       "L'adhérence",                SurahRevelationType.M),
                new SurahDescription(97,  5,    "Al-Qadr",       "La Destinée",                SurahRevelationType.M),
                new SurahDescription(98,  8,    "Al-Bayyina",    "La preuve",                  SurahRevelationType.H),
                new SurahDescription(99,  8,    "Az-Zalzala",    "La secousse",                SurahRevelationType.H),
                new SurahDescription(100, 11,   "Al-Adiyat",     "Les coursiers",              SurahRevelationType.M),
                new SurahDescription(101, 11,   "Al-Qariah",     "Le fracas",                  SurahRevelationType.M),
                new SurahDescription(102, 8,    "At-Takatur",    "La course aux richesses",    SurahRevelationType.M),
                new SurahDescription(103, 3,    "Al-Asr",        "Le temps",                   SurahRevelationType.M),
                new SurahDescription(104, 9,    "Al-Humazah",    "Les calomniateurs",          SurahRevelationType.M),
                new SurahDescription(105, 5,    "Al-Fil",        "L'éléphant",                 SurahRevelationType.M),
                new SurahDescription(106, 4,    "Quraysh",       "Les Quraychites",            SurahRevelationType.M),
                new SurahDescription(107, 7,    "Al-Maun",       "L'ustensile",                SurahRevelationType.M),
                new SurahDescription(108, 3,    "Al-Kawthar",    "L'abondance",                SurahRevelationType.M),
                new SurahDescription(109, 6,    "Al-Kafirun",    "Les infidèles",              SurahRevelationType.M),
                new SurahDescription(110, 3,    "An-Nasr",       "Les secours",                SurahRevelationType.H),
                new SurahDescription(111, 5,    "Al-Masad",      "Les fibres",                 SurahRevelationType.M),
                new SurahDescription(112, 4,    "Al-Ikhlas",     "Le monothéisme pur",         SurahRevelationType.M),
                new SurahDescription(113, 5,    "Al-Falaq",      "L'aube naissante",           SurahRevelationType.M),
                new SurahDescription(114, 6,    "An-Nas",        "Les Hommes",                 SurahRevelationType.M)
            };

            string translationPath = @"C:\Users\jomtek360\Documents\Baraka\quran-translated-main";
            
            for (int i = 0; i < 114; i++)
            {
                var desc = SurahList[i];
                var surahNum = desc.SurahNumber;
                var arVersion = File.ReadAllLines($@"{translationPath}\arabic_uthmani\{surahNum}");
                var phVersion = File.ReadAllLines($@"{translationPath}\phonetic\{surahNum}");

                SerializationData.Add(desc, new Dictionary<string, SurahVersion>()
                {
                    ["ARABIC"] = new SurahVersion("ARABIC", arVersion),
                    ["PHONETIC"] = new SurahVersion("PHONETIC", phVersion)
                });
            }
            Data.SerializationUtils.Serialize(SerializationData, "qurannew.ser");
        }
        #endregion
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //SerializeQuran();
            //return;

            TitleTB.Margin = new Thickness(
                TitleTB.Margin.Left,
                50,
                TitleTB.Margin.Right,
                TitleTB.Margin.Bottom
            );

            var c =
                SerializationUtils.Deserialize<MySettings>("settings.ser");
            LoadedData.Settings = c;

            if (!LoadedData.Settings.ShowWelcomeWindow)
            {
                Visibility = Visibility.Hidden;
            }

            MainPB.Visibility = Visibility.Collapsed;
            ProgressTB.Visibility = Visibility.Collapsed;

            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(800);

            TitleTB.Margin = new Thickness(
                TitleTB.Margin.Left,
                10,
                TitleTB.Margin.Right,
                TitleTB.Margin.Bottom
            );

            MainPB.Visibility = Visibility.Visible;
            ProgressTB.Visibility = Visibility.Visible;

            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(10);

            // // Loading
            //
            // Deserialize data
            ProgressTB.Text = "chargement des ressources: cheikh.ser";
            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(25);
            LoadedData.CheikhList =
                SerializationUtils.Deserialize<CheikhDescription[]>("data/cheikh.ser");     
            MainPB.Progress = 0.1;

            ProgressTB.Text = "chargement des ressources: quran/translations_cache.ser";
            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(25);
            LoadedData.SurahList =
                SerializationUtils.Deserialize<Dictionary<SurahDescription, Dictionary<string, SurahVersion>>>("data/quran/translations_cache.ser");
            MainPB.Progress = 0.25;

            ProgressTB.Text = "chargement des ressources: quran/translations_info.ser";
            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(25);
            LoadedData.TranslationsList =
                SerializationUtils.Deserialize<TranslationDescription[]>("data/quran/translations_info.ser");
            MainPB.Progress = 0.4;

            ProgressTB.Text = "chargement du cache...";
            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(50);
            var cacheContent =
                SerializationUtils.Deserialize<Dictionary<string, byte[]>>("data/cache.ser");
            LoadedData.AudioCache = new AudioCacheManager(cacheContent);
            // DEBUG -- LoadedData.AudioCache = new AudioCacheManager(new Dictionary<string, byte[]>());
            MainPB.Progress = 0.6;

            ProgressTB.Text = "chargement du Mushaf...";
            LoadedData.MushafFontManager = new MushafFontManager();
            LoadedData.MushafGlyphProvider = new MushafGlyphProvider();
            //LoadedData.MushafGlyphProvider.LoadGlyphInfo(); // TEMPORARY
            LoadedData.MushafGlyphProvider.GlyphInfoDict =
                SerializationUtils.Deserialize<Dictionary<(int, char), MushafGlyphDescription>>("data/quran/mushaf/glyph_info.ser");

            // Deserialize settings
            ProgressTB.Text = "chargement des marque-pages...";
            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(50);
            // DEBUG -- LoadedData.Bookmarks = (new int[114]).ToList();
            LoadedData.Bookmarks = SerializationUtils.Deserialize<List<int>>("bookmarks.ser");
            
            //  // Finish
            ProgressTB.Text = $"préparation de l'interface...";
            if (LoadedData.Settings.ShowWelcomeWindow) await Task.Delay(25);

            var window = new MainWindow();
            window.Visibility = Visibility.Hidden;
            window.ContentRendered += Window_ContentRendered;
            window.LoadingProgressChanged += Window_LoadingProgressChanged;

            App.Current.MainWindow = window;
            window.Show();
        }

        #region MainWindow load-specific events
        private async void Window_ContentRendered(object sender, EventArgs e)
        {
            var instance = sender as MainWindow;

            var defaultCheikh = LoadedData.CheikhList.ElementAt(LoadedData.Settings.DefaultCheikhIndex);
            var defaultSurah = LoadedData.SurahList.ElementAt(LoadedData.Settings.DefaultSurahIndex).Key;

            instance.Player.ChangeSelectedCheikh(defaultCheikh);
            await instance.Player.ChangeSelectedSurahAsync(defaultSurah, true, true, true);

            if (LoadedData.Settings.SurahVersionConfig.ShowMushaf())
            {
                await instance.ChangeDisplayModeAsync(QuranDisplayMode.MUSHAF);
            }
            else
            {
                await instance.ChangeDisplayModeAsync(QuranDisplayMode.TRANSLATED);
            }

            // Other UI settings
            instance.Player.PrepareItems();
            instance.TranslatedSurahDisplayer.SetSBVisible(LoadedData.Settings.DisplayScrollBar);
        }

        private void Window_LoadingProgressChanged(object sender, double progress)
        {
            var instance = sender as MainWindow;

            int percentage = (int)Math.Round(progress * 100);

            Dispatcher.Invoke(new Action(() =>
            {
                ProgressTB.Text = $"préparation de l'interface... {percentage}%";
                MainPB.Progress = 0.6 + (0.4 * progress);
            }), DispatcherPriority.ContextIdle);

            if (percentage == 100)
            {
                instance.Show();
                instance.Activate(); // Bring window to front
                this.Close(); // Close welcome window
            }
        }
        #endregion

        #region DEBUG
        /*
        internal class Stats
        {
            public int deletions { get; set; }
            public int transpositions { get; set; }
            public int insertions { get; set; }
        }

        internal class DummySegmentClass
        {
            public int ayah { get; set; }
            public int surah { get; set; }
            public List<List<int>> segments { get; set; }
            public Stats stats
            {
                get; set;
            }
        }
            // Load individual cheikh wav segments

            var newCheikhData = new List<CheikhDescription>();

            foreach (var cheikhDesc in LoadedData.CheikhList)
            {
                string fileId = cheikhDesc.StreamingUrl.Replace("https://everyayah.com/data/", "");

                if (!File.Exists($@"C:\Users\jomtek360\Documents\Baraka\align_data\{fileId}.json"))
                {
                    newCheikhData.Add(cheikhDesc);
                    continue;
                }

                string strJsonContent = File.ReadAllText($@"C:\Users\jomtek360\Documents\Baraka\align_data\{fileId}.json");

                var jsonContent = JsonConvert.DeserializeObject<DummySegmentClass[]>(strJsonContent);
                var segmentsDict = new Dictionary<(int, int), List<int>>();

                foreach (DummySegmentClass entry in jsonContent)
                {
                    var verseDesc = (entry.surah, entry.ayah - 1);
                    var segments = new List<int>();

                    if (entry.segments != null)
                    {
                        foreach (var segment in entry.segments)
                        {
                            segments.Add(segment[3]);
                        }
                    }

                    segmentsDict.Add(verseDesc, segments);
                }

                var wavSegData = new Data.Cheikh.WavSegmentationData(segmentsDict);

                newCheikhData.Add(
                    new CheikhDescription(cheikhDesc.FirstName, cheikhDesc.LastName, cheikhDesc.StreamingUrl, cheikhDesc.GetPhoto(), wavSegData));
            }

            SerializationUtils.Serialize(newCheikhData.ToArray(), "data/newcheikh.ser");

            MessageBox.Show("done");
            return;
        */
        #endregion
    }
}
