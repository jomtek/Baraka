using Baraka.Data.Descriptions;
using Baraka.Data.Surah;
using System.Collections.Generic;

namespace Baraka.Data
{
    public static class LoadedData
    {
        // Data
        public static Dictionary<SurahDescription, Dictionary<string, SurahVersion>> SurahList;
        public static CheikhDescription[] CheikhList;
        public static TranslationDescription[] TranslationsList;

        // Settings
        public static MySettings Settings;
        public static List<int> Bookmarks;

        // Cache
        public static AudioCacheManager AudioCache;
    }
}