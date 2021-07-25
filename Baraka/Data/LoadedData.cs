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

        // Hardcoded data
        public static List<(HizbDescription, HizbDescription)> JuzAndHizb =
            new List<(HizbDescription, HizbDescription)>()
            {
                // start_s, end_s, start_v, end_v
                (new HizbDescription(1,  2,  1,   74,  1 ),  new HizbDescription(2,  2,   75,  141, 2 )),
                (new HizbDescription(2,  2,  142, 202, 3 ),  new HizbDescription(2,  2,   203, 252, 4 )),
                (new HizbDescription(2,  3,  253, 14,  5 ),  new HizbDescription(3,  3,   15,  92,  6 )),
                (new HizbDescription(3,  3,  93,  170, 7 ),  new HizbDescription(3,  4,   171, 23,  8 )),
                (new HizbDescription(4,  4,  24,  87,  9 ),  new HizbDescription(4,  4,   88,  147, 10)),
                (new HizbDescription(4,  5,  148, 26,  11),  new HizbDescription(5,  5,   27,  81,  12)),
                (new HizbDescription(5,  6,  82,  35,  13),  new HizbDescription(6,  6,   36,  110, 14)),
                (new HizbDescription(6,  6,  111, 165, 15),  new HizbDescription(7,  7,   1,   87,  16)),
                (new HizbDescription(7,  7,  88,  170, 17),  new HizbDescription(7,  8,   171, 40,  18)),
                (new HizbDescription(8,  9,  41,  33,  19),  new HizbDescription(9,  9,   34,  92,  20)),
                (new HizbDescription(9,  10, 93,  25,  21),  new HizbDescription(10, 11,  26,  5,   22)),
                (new HizbDescription(11, 11, 6,   83,  23),  new HizbDescription(11, 12,  84,  52,  24)),
                (new HizbDescription(12, 13, 53,  18,  25),  new HizbDescription(13, 14,  19,  52,  26)),
                (new HizbDescription(15, 16, 1,   50,  27),  new HizbDescription(16, 16,  51,  128, 28)),
                (new HizbDescription(17, 17, 1,   98,  29),  new HizbDescription(17, 18,  99,  74,  30)),
                (new HizbDescription(18, 19, 75,  98,  31),  new HizbDescription(20, 20,  1,   135, 32)),
                (new HizbDescription(21, 21, 1,   112, 33),  new HizbDescription(22, 22,  1,   78,  34)),
                (new HizbDescription(23, 24, 1,   20,  35),  new HizbDescription(24, 25,  21,  21,  36)),
                (new HizbDescription(25, 26, 22,  110, 37),  new HizbDescription(26, 27,  111, 55,  38)),
                (new HizbDescription(27, 28, 56,  50,  39),  new HizbDescription(28, 29,  51,  45,  40)),
                (new HizbDescription(29, 31, 46,  21,  41),  new HizbDescription(31, 33,  22,  30,  42)),
                (new HizbDescription(33, 34, 31,  23,  43),  new HizbDescription(34, 36,  24,  27,  44)),
                (new HizbDescription(36, 37, 28,  114, 45),  new HizbDescription(37, 39,  145, 31,  46)),
                (new HizbDescription(39, 40, 32,  40,  47),  new HizbDescription(40, 41,  41,  46,  48)),
                (new HizbDescription(41, 43, 47,  23,  49),  new HizbDescription(43, 45,  24,  37,  50)),
                (new HizbDescription(46, 48, 1,   17,  51),  new HizbDescription(48, 51,  18,  30,  52)),
                (new HizbDescription(51, 54, 31,  55,  53),  new HizbDescription(55, 57,  1,   29,  54)),
                (new HizbDescription(58, 61, 1,   14,  55),  new HizbDescription(62, 66,  1,   12,  56)),
                (new HizbDescription(67, 71, 1,   28,  57),  new HizbDescription(72, 77,  1,   50,  58)),
                (new HizbDescription(78, 86, 1,   17,  59),  new HizbDescription(87, 114, 1,   6,   60)),
            };
    }
}