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
    }
}
