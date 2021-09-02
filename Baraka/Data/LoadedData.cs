using Baraka.Data.Descriptions;
using Baraka.Data.Quran;
using Baraka.Data.Quran.Mushaf;
using Baraka.Data.Surah;
using Baraka.Theme.UserControls.Quran.Display.Mushaf;
using Baraka.Theme.UserControls.Quran.Display.Mushaf.Data;
using System.Collections.Generic;

namespace Baraka.Data
{
    public static class LoadedData
    {
        // Data
        //
        public static Dictionary<SurahDescription, Dictionary<string, SurahVersion>> SurahList;
        public static CheikhDescription[] CheikhList;
        public static TranslationDescription[] TranslationsList;
// Mushaf data
        public static MushafFontManager MushafFontManager;
        public static MushafGlyphProvider MushafGlyphProvider;

        // Settings
        public static MySettings Settings;
        public static List<int> Bookmarks;

        // Cache
        public static AudioCacheManager AudioCache;

        // Hardcoded data
        //
        // Juz and hizb list
        // Each line is a Juz, composed of a Tuple containing 2 hizb
        public static List<(HizbDescription, HizbDescription)> JuzAndHizb =
            new List<(HizbDescription, HizbDescription)>()
            {
                // start_s, end_s, start_v, end_v
                (new HizbDescription(001, 002, 001, 074, 001),  new HizbDescription(002, 002, 075, 141, 002)),
                (new HizbDescription(002, 002, 142, 202, 003),  new HizbDescription(002, 002, 203, 252, 004)),
                (new HizbDescription(002, 003, 253, 014, 005),  new HizbDescription(003, 003, 015, 092, 006)),
                (new HizbDescription(003, 003, 093, 170, 007),  new HizbDescription(003, 004, 171, 023, 008)),
                (new HizbDescription(004, 004, 024, 087, 009),  new HizbDescription(004, 004, 088, 147, 010)),
                (new HizbDescription(004, 005, 148, 026, 011),  new HizbDescription(005, 005, 027, 081, 012)),
                (new HizbDescription(005, 006, 082, 035, 013),  new HizbDescription(006, 006, 036, 110, 014)),
                (new HizbDescription(006, 006, 111, 165, 015),  new HizbDescription(007, 007, 001, 087, 016)),
                (new HizbDescription(007, 007, 088, 170, 017),  new HizbDescription(007, 008, 171, 040, 018)),
                (new HizbDescription(008, 009, 041, 033, 019),  new HizbDescription(009, 009, 034, 092, 020)),
                (new HizbDescription(009, 010, 093, 025, 021),  new HizbDescription(010, 011, 026, 005, 022)),
                (new HizbDescription(011, 011, 006, 083, 023),  new HizbDescription(011, 012, 084, 052, 024)),
                (new HizbDescription(012, 013, 053, 018, 025),  new HizbDescription(013, 014, 019, 052, 026)),
                (new HizbDescription(015, 016, 001, 050, 027),  new HizbDescription(016, 016, 051, 128, 028)),
                (new HizbDescription(017, 017, 001, 098, 029),  new HizbDescription(017, 018, 099, 074, 030)),
                (new HizbDescription(018, 019, 075, 098, 031),  new HizbDescription(020, 020, 001, 135, 032)),
                (new HizbDescription(021, 021, 001, 112, 033),  new HizbDescription(022, 022, 001, 078, 034)),
                (new HizbDescription(023, 024, 001, 020, 035),  new HizbDescription(024, 025, 021, 021, 036)),
                (new HizbDescription(025, 026, 022, 110, 037),  new HizbDescription(026, 027, 111, 055, 038)),
                (new HizbDescription(027, 028, 056, 050, 039),  new HizbDescription(028, 029, 051, 045, 040)),
                (new HizbDescription(029, 031, 046, 021, 041),  new HizbDescription(031, 033, 022, 030, 042)),
                (new HizbDescription(033, 034, 031, 023, 043),  new HizbDescription(034, 036, 024, 027, 044)),
                (new HizbDescription(036, 037, 028, 114, 045),  new HizbDescription(037, 039, 145, 031, 046)),
                (new HizbDescription(039, 040, 032, 040, 047),  new HizbDescription(040, 041, 041, 046, 048)),
                (new HizbDescription(041, 043, 047, 023, 049),  new HizbDescription(043, 045, 024, 037, 050)),
                (new HizbDescription(046, 048, 001, 017, 051),  new HizbDescription(048, 051, 018, 030, 052)),
                (new HizbDescription(051, 054, 031, 055, 053),  new HizbDescription(055, 057, 001, 029, 054)),
                (new HizbDescription(058, 061, 001, 014, 055),  new HizbDescription(062, 066, 001, 012, 056)),
                (new HizbDescription(067, 071, 001, 028, 057),  new HizbDescription(072, 077, 001, 050, 058)),
                (new HizbDescription(078, 086, 001, 017, 059),  new HizbDescription(087, 114, 001, 006, 060)),
            };

        // Sujood places list (seed=1:1)
        public static List<VerseDescription> SujoodVerses
        {
            get
            {
                return new List<VerseDescription>() {
                    new VerseDescription(Utils.Quran.General.FindSurah(07), 206),
                    new VerseDescription(Utils.Quran.General.FindSurah(13), 15),
                    new VerseDescription(Utils.Quran.General.FindSurah(16), 50),
                    new VerseDescription(Utils.Quran.General.FindSurah(17), 109),
                    new VerseDescription(Utils.Quran.General.FindSurah(19), 58),
                    new VerseDescription(Utils.Quran.General.FindSurah(22), 18),
                    new VerseDescription(Utils.Quran.General.FindSurah(22), 77),
                    new VerseDescription(Utils.Quran.General.FindSurah(25), 60),
                    new VerseDescription(Utils.Quran.General.FindSurah(27), 26),
                    new VerseDescription(Utils.Quran.General.FindSurah(32), 15),
                    new VerseDescription(Utils.Quran.General.FindSurah(38), 24),
                    new VerseDescription(Utils.Quran.General.FindSurah(41), 38),
                    new VerseDescription(Utils.Quran.General.FindSurah(53), 62),
                    new VerseDescription(Utils.Quran.General.FindSurah(84), 21),
                    new VerseDescription(Utils.Quran.General.FindSurah(96), 19),
                };
            }
        }


    }
}