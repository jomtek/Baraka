using Baraka.Data.Surah;
using Baraka.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data
{
    public class MySettings
    {
        private string _path;

        // With temporary (debug) default values

        #region Values
        // Settings window (clear between sessions)
        public int SelectedTab { get; set; } = 0;

        // General
        public bool Startup { get; set; } = false;
        public Language Language { get; set; } = Language.FRENCH;
        public bool EnableAudioCache { get; set; } = true;
        public bool ClearAudioCache { get; set; } = false;

        // Appearance
        public bool ShowWelcomeWindow { get; set; } = true;
        public bool DisplayScrollBar { get; set; } = true;

        // Reading
        public int DefaultSurahIndex { get; set; } = 0;
        public int DefaultCheikhIndex { get; set; } = 0;
        public bool AutoScrollQuran { get; set; } = false;
        public bool AutoNextSurah { get; set; } = false;
        public bool AutoReloadLastSurah { get; set; } = true;
        public int CrossFadingValue { get; set; } = 10;
        public string OutputDeviceGuid { get; set; } =
            "010000000-0000-0000-0000-000000000000"; // Default output
        public SurahVersionConfig SurahVersionConfig =
            new SurahVersionConfig(true, false, true, LoadedData.TranslationsList[37] /*fr.hamidullah (debug)*/, null, null);

        // Search
        public string SearchEdition { get; set; } = "ARABIC";
        public string ResultsEdition { get; set; } = "ARABIC";
        public bool HighlightSearchKeywords { get; set; } = true;
        #endregion


        public MySettings(string path)
        {
            _path = path;
        }
        public void Save()
        {
            // use _path
        }
    }
}
