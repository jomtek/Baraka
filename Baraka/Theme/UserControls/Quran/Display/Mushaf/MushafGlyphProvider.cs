using Baraka.Data;
using Baraka.Data.Descriptions;
using Baraka.Data.Quran.Mushaf;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Windows.Controls;

namespace Baraka.Theme.UserControls.Quran.Display.Mushaf
{
    public class MushafGlyphProvider
    {
        // (page [seed=1], decoded glyph) >> glyph description
        public Dictionary<(int, char), MushafGlyphDescription> GlyphInfoDict { get; set; }

        // `page` starts from 1
        public IEnumerable<List<MushafGlyphDescription>> RetrievePage(int page)
        {
            if (GlyphInfoDict == null)
                throw new ArgumentException();

            List<MushafDbQuery> lines;
            using (IDbConnection cnn = new SQLiteConnection(Utils.Quran.DB.LoadConnectionString("MadaniQuran")))
            {
                string query = $"select page, sura, ayah, text, line from madani_page_text where page={page}";
                lines = cnn.Query<MushafDbQuery>(query, new DynamicParameters()).ToList();
            };

            foreach (MushafDbQuery line in lines)
            {
                var glyphs = new List<MushafGlyphDescription>(); // Glyphs for current line
                foreach (char glyph in WebUtility.HtmlDecode(line.text))
                {
                    glyphs.Add(GlyphInfoDict[(page, glyph)]);
                }

                yield return glyphs;
            }
        }

        // `sura` starts from 1
        public List<List<MushafGlyphDescription>> RetrieveSurah(int sura)
        {
            if (GlyphInfoDict == null)
                throw new ArgumentException();

            var glyphs = new List<List<MushafGlyphDescription>>();

            List<MushafDbQuery> verses;
            using (IDbConnection cnn = new SQLiteConnection(Utils.Quran.DB.LoadConnectionString("MadaniQuran")))
            {
                string query = $"select page, sura, ayah, text from sura_ayah_page_text where sura={sura}";
                verses = cnn.Query<MushafDbQuery>(query, new DynamicParameters()).ToList();
            };

            foreach (MushafDbQuery verse in verses)
            {
                var currentVerseGlyphs = new List<MushafGlyphDescription>(); // Glyphs for current verse
                foreach (char glyph in WebUtility.HtmlDecode(verse.text))
                {
                    currentVerseGlyphs.Add(GlyphInfoDict[(verse.page, glyph)]);
                }

                glyphs.Add(currentVerseGlyphs);
            }

            return glyphs;
        }

        #region DEBUG
        // This method is TEMPORARY and will be wiped out soon
        public void LoadGlyphInfo()
        {
            // Load _glyphInfoDict

            for (int page = 1; page < 605; page++)
            {
                List<MushafDbQuery> verses;
                using (IDbConnection cnn = new SQLiteConnection(Utils.Quran.DB.LoadConnectionString("MadaniQuran")))
                {
                    string query = $"select page, sura, ayah, text from sura_ayah_page_text where page={page}";
                    verses = cnn.Query<MushafDbQuery>(query, new DynamicParameters()).ToList();
                };

                // Fill the dictionary with all the glyphs of the mushaf
                // During this stage, we should resolve the following data for each glyph:
                // - Decoded data
                // - Associated verse
                // - Type
                foreach (MushafDbQuery verse in verses)
                {
                    string glyphArray = WebUtility.HtmlDecode(verse.text);
                    int glyphPos = 0;
                    foreach (char glyph in glyphArray)
                    {
                        // Prepare needed data
                        var associatedVerse =
                            new VerseDescription(Utils.Quran.General.FindSurah(verse.sura), verse.ayah);

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
                        // Associated verse is sujood and glyph is just before the last position
                        else if (LoadedData.SujoodVerses.Contains(associatedVerse) && glyphPos == glyphArray.Length - 2)
                        {
                            glyphType = MushafGlyphType.SUJOOD;
                        }
                        else
                        {
                            List<int> qres;
                            using (IDbConnection cnn = new SQLiteConnection(Utils.Quran.DB.LoadConnectionString("MadaniQuran")))
                            {
                                string query = $"select ayah from sura_ayah_info where sura={verse.sura} and ayah={verse.ayah} and hizb=1";
                                qres = cnn.Query<int>(query, new DynamicParameters()).ToList();
                            };

                            // Current glyph is at position 0, isn't 1:x nor x:1, and, by deduction, contains a rub-al-hizb mark
                            if (qres.Count != 0 && glyphPos == 0 && verse.sura != 1 && verse.ayah != 1)
                            {
                                glyphType = MushafGlyphType.RUB_EL_HIZB;
                            }
                            else
                            {
                                var arabicTB = new TextBlock();
                                arabicTB.FontFamily = LoadedData.MushafFontManager.FindPageFontFamily(verse.page);

                                if (Utils.General.MeasureText(glyph.ToString(), arabicTB) < 3)
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
                        var description = new MushafGlyphDescription(glyph, associatedVerse, glyphType, page);
                        GlyphInfoDict.Add((verse.page, glyph), description);

                        glyphPos++;
                    }
                }

                System.Console.WriteLine($"Done with page {page}");
            }

            System.Console.WriteLine("done completely");
        }
        #endregion
    }
}
