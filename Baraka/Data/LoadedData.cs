using Baraka.Data.Descriptions;
using System.Collections.Generic;

namespace Baraka.Data
{
    public static class LoadedData
    {
        public static Dictionary<SurahDescription, Surah.SurahVersion[]> SurahList;
        public static CheikhDescription[] CheikhList;
        public static MySettings Settings;
    }
}
