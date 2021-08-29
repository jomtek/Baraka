using Baraka.Data;
using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Utils.Quran
{
    public static class General
    {
        public static SurahDescription FindSurah(int num) // num starts from 1
        {
            return LoadedData.SurahList.ElementAt(num - 1).Key;
        }

        // Gives information about:
        // - Sura number
        // - Sura phonetic and translated name
        // - Number of verses the sura contains
        // - Sura revelation type (makka or madina)
        public static string GenerateSynopsis(SurahDescription surah)
        {
            var sb = new StringBuilder();

            string revelationType;
            switch (surah.RevelationType)
            {
                case SurahRevelationType.M:
                    revelationType = "mecquoise (La Mecque)";
                    break;
                case SurahRevelationType.H:
                    revelationType = "médinoise (Médine)";
                    break;
                default:
                    revelationType = "mecquoise ou médinoise";
                    break;
            }

            sb.AppendLine($"Sourate {surah.SurahNumber}: {surah.PhoneticName} ({surah.TranslatedName})");
            sb.AppendLine($"Contient {surah.NumberOfVerses} versets");
            sb.Append($"Révélation {revelationType}");

            return sb.ToString();
        }

        public static string PrettyPrintVerse(int verseNum, SurahDescription surah)
        {
            //if (verseNum == 0) verseNum = 1;

            var firstTransId =
                LoadedData.TranslationsList[LoadedData.Settings.SurahVersionConfig.Translation1].Identifier;
            var surahVer = LoadedData.SurahList[surah][firstTransId];
            string verse = surahVer.Verses[verseNum];

            if (surah.HasBasmala())
            {
                if (verseNum == 0)
                {
                    verse = LoadedData.SurahList.ElementAt(0).Value[firstTransId].Verses[0];
                }
                else
                {
                    verse = surahVer.Verses[verseNum - 1];
                }
            }
            else
            {
                verseNum++;
            }

            return $"{surah.SurahNumber}:{verseNum} {verse}";
        }
    }
}
