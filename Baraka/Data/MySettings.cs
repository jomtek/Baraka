using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using Baraka.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data
{
    [Serializable]
    public class MySettings
    {
        // With temporary (debug) default values

        #region Values
        // General
        public bool Startup { get; set; } = false;
        public Language Language { get; set; } = Language.FRENCH;
        public bool EnableAudioCache { get; set; } = true;
        public bool ClearAudioCache { get; set; } = false;

        // Appearance
        public bool ShowWelcomeWindow { get; set; } = true;
        public bool DisplayScrollBar { get; set; } = true;

        // Reading
        public int DefaultSurahIndex { get; set; } = 0; // Al-Fatiha
        public int DefaultCheikhIndex { get; set; } = 4; // Saad Al-Ghamadi
        public bool AutoScrollQuran { get; set; } = false;
        public bool AutoNextSurah { get; set; } = false;
        public bool AutoReloadLastSurah { get; set; } = true;
        public int CrossFadingValue { get; set; } = 5;
        public int OutputDeviceIndex { get; set; } = 0; // Default output
        public SurahVersionConfig SurahVersionConfig =
            new SurahVersionConfig(true, false, true, -1, -1, -1);

        // Search
        public string SearchEdition { get; set; } = "ARABIC";
        public string ResultsEdition { get; set; } = "ARABIC";
        public bool HighlightSearchKeywords { get; set; } = true;
        #endregion
    }
}
