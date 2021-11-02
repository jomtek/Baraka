using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Models.Quran.Configuration
{
    public class EditionConfigModel
    {
        public readonly bool AllowArabic;
        public readonly bool AllowTransliteration;
        public readonly string Translation1;
        public readonly string Translation2;
        public readonly string Translation3;

        public EditionConfigModel(
            bool allowArabic,
            bool allowTransliteration,
            string translation1,
            string translation2,
            string translation3)
        {
            AllowArabic = allowArabic;
            AllowTransliteration = allowTransliteration;
            Translation1 = translation1;
            Translation2 = translation2;
            Translation3 = translation3;
        }
    }
}
