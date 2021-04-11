using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Streaming
{
    public static class StreamingUtils
    {
        public static string GenerateVerseUrl(CheikhDescription cheikh, SurahDescription surah, int verseNum)
        {
            return $"{cheikh.StreamingUrl}/{surah.SurahNumber.ToString("000")}{verseNum.ToString("000")}.mp3";
        }
    }
}
