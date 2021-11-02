using Baraka.Models.Quran;
using Baraka.Models.Quran.Mushaf;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Data;
using Baraka.Utils;

namespace Baraka.Services.Quran.Mushaf
{
    public static class MushafGlyphService
    {
        // TODO: adapt to online API
        private static readonly string _dbConnectionString = @"Data Source=glyphmap.db;Version=3;Synchronous=Off;UTF8Encoding=True;";

        // (page [seed=1], decoded glyph) >> glyph description
        private static Dictionary<(int, char), MushafGlyphModel> _glyphInfoDict;

        private static readonly List<VerseLocationModel> _sujoodVerses = new()
        {
            new VerseLocationModel(7,  206),
            new VerseLocationModel(13, 15),
            new VerseLocationModel(16, 50),
            new VerseLocationModel(17, 109),
            new VerseLocationModel(19, 58),
            new VerseLocationModel(22, 18),
            new VerseLocationModel(22, 77),
            new VerseLocationModel(25, 60),
            new VerseLocationModel(27, 26),
            new VerseLocationModel(32, 15),
            new VerseLocationModel(38, 24),
            new VerseLocationModel(41, 38),
            new VerseLocationModel(53, 62),
            new VerseLocationModel(84, 21),
            new VerseLocationModel(96, 19),
        };

        public static void LoadGlyphsFromFile(string path)
        {
            _glyphInfoDict = Serialization.Deserialize<Dictionary<(int, char), MushafGlyphModel>>(path);
        }

        // `page` starts from 1
        public static List<MushafGlyphModel[]> RetrievePage(int page)
        {
            throw new NotImplementedException();
        }

        // `sura` starts from 1
        public static List<MushafGlyphModel[]> RetrieveSura(SuraModel sura)
        {
            if (_glyphInfoDict == null)
                throw new ArgumentException();

            var versesGlyphs = new List<MushafGlyphModel[]>();

            List<MushafDbEntry> verses;
            using (IDbConnection cnn = new SQLiteConnection(_dbConnectionString))
            {
                string query = $"select page, sura, ayah, text from sura_ayah_page_text where sura={sura.Number}";
                verses = cnn.Query<MushafDbEntry>(query, new DynamicParameters()).ToList();
            };

            foreach (MushafDbEntry verse in verses)
            {
                var currentVerseGlyphs = new List<MushafGlyphModel>();
                foreach (char glyph in WebUtility.HtmlDecode(verse.text))
                {
                    currentVerseGlyphs.Add(_glyphInfoDict[(verse.page, glyph)]);
                }

                versesGlyphs.Add(currentVerseGlyphs.ToArray());
            }

            return versesGlyphs;
        }

        #region DEBUG
        /* This method is UNUSED and is kept here only for maintenance purposes...
            This method generates a dictionary which contains information about the nature of each single glyph...
            Information is deducted using various techniques, such as counting, measuring text or even using references... */
        public static void GenerateGlyphInfo()
        {
            _glyphInfoDict = new Dictionary<(int, char), MushafGlyphModel>();

            for (int page = 1; page < 605; page++)
            {
                System.Diagnostics.Trace.WriteLine($"generating page {page}");
                List<MushafDbEntry> verses;

                using (IDbConnection cnn = new SQLiteConnection(_dbConnectionString))
                {
                    string query = $"select page, sura, ayah, text from sura_ayah_page_text where page={page}";
                    verses = cnn.Query<MushafDbEntry>(query, new DynamicParameters()).ToList();
                };

                // Fill the dictionary with all the glyphs of the mushaf
                // During this stage, we should resolve the following data for each glyph:
                // - Decoded data
                // - Associated verse
                // - Type
                foreach (MushafDbEntry verse in verses)
                {
                    string glyphArray = WebUtility.HtmlDecode(verse.text);
                    int glyphPos = 0;
                    foreach (char glyph in glyphArray)
                    {
                        // Prepare needed data
                        var verseLoc = new VerseLocationModel(verse.sura, verse.ayah);

                        MushafGlyphType glyphType;
                        if (verse.ayah == 0)
                        {
                            if (glyphArray.Length == 2)
                            {
                                glyphType = MushafGlyphType.SURA_NAME;
                            }
                            else
                            {
                                glyphType = MushafGlyphType.BASMALA;
                            }
                        }
                        else if (glyphPos == glyphArray.Length - 1)
                        {
                            glyphType = MushafGlyphType.END_OF_AYAH;
                        }
                        else if (_sujoodVerses.Contains(verseLoc) && glyphPos == glyphArray.Length - 2)
                        {
                            // Associated verse is sujood and glyph is just before the last position
                            glyphType = MushafGlyphType.SUJOOD;
                        }
                        else
                        {
                            List<int> qres;
                            using (IDbConnection cnn = new SQLiteConnection(_dbConnectionString))
                            {
                                string query = $"select ayah from sura_ayah_info where sura={verse.sura} and ayah={verse.ayah} and hizb=1";
                                qres = cnn.Query<int>(query, new DynamicParameters()).ToList();
                            };

                            if (qres.Count != 0 && glyphPos == 0 && verse.sura != 1 && verse.ayah != 1)
                            {
                                // Current glyph is at position 0, isn't part of 1:x nor x:1, and, by deduction, is a rub-al-hizb mark
                                glyphType = MushafGlyphType.RUB_EL_HIZB;
                            }
                            else
                            {
                                var arabicTB = new TextBlock()
                                {
                                    FontFamily = MushafFontService.FindPageFontFamily(verse.page)
                                };

                                if (Utils.UI.Graphics.MeasureText(glyph.ToString(), arabicTB) < 3)
                                {
                                    glyphType = MushafGlyphType.STOPPING_SIGN;
                                }
                                else
                                {
                                    glyphType = MushafGlyphType.WORD;
                                }
                            }
                        }

                        // Fill dictionary
                        var description = new MushafGlyphModel(glyph, verseLoc, glyphType, page);
                        _glyphInfoDict.Add((verse.page, glyph), description);

                        glyphPos++;
                    }
                }
            }
        }

        public static void SaveGlyphsToPath(string path)
        {
            Serialization.Serialize(_glyphInfoDict, path);
        }
        #endregion
    }
}
