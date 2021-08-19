using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.IO;
using Baraka.Theme.UserControls.Quran.Display.Mushaf;
using System.Data;
using System.Data.SQLite;
using Dapper;
using Baraka.Data.Descriptions;

namespace Baraka.Data.Quran
{
    public class MushafDataManager
    {
        // All the fonts belong to the Saudi King Fahd complex and were edited to suit Baraka's needs
        private string _fontsPath;

        public MushafDataManager()
        {
            _fontsPath =
                Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName +
                @"\data\quran\mushaf\fonts";
        }

        public FontFamily FindPageFontFamily(int pageIdx)
        {
            string fontName = $"QCF_P{(pageIdx + 1).ToString("000")}";
            return new FontFamily($@"{_fontsPath}\#{fontName}");
        }

        public List<MadaniMushafLine> LineLookup(string condition)
        {
            List<MadaniMushafLine> lines;
            using (IDbConnection cnn = new SQLiteConnection(Utils.Quran.DB.LoadConnectionString("MadaniQuran")))
            {
                string query = $"select page, line, sura, ayah, text from madani_page_text where {condition}";
                lines = cnn.Query<MadaniMushafLine>(query, new DynamicParameters()).ToList();
            };

            return lines;
        }

        
        public Dictionary<VerseDescription, MadaniMushafVerse> VerseLookup(string condition)
        {
            var results = new Dictionary<VerseDescription, MadaniMushafVerse>();

            List<MadaniMushafVerse> verses;
            using (IDbConnection cnn = new SQLiteConnection(Utils.Quran.DB.LoadConnectionString("MadaniQuran")))
            {
                string query = $"select sura, ayah, page, text from sura_ayah_page_text where {condition}";
                verses = cnn.Query<MadaniMushafVerse>(query, new DynamicParameters()).ToList();
            };

            foreach (MadaniMushafVerse verse in verses)
            {
                var description = new VerseDescription(Utils.Quran.General.FindSurah(verse.sura), verse.ayah);
                results.Add(description, verse);
            }

            return results;
        }
        
    }
}
