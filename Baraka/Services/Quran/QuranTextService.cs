using Baraka.Models;
using Baraka.Models.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Services.Quran
{
    public static class QuranTextService
    {
        private static string[] LoadEdition(int suraNumber, string id)
        {
            var api = (string)App.Current.FindResource("API_PATH");
            // TODO: adapt to the online api
            return File.ReadAllLines($@"{api}\text\{id}\{suraNumber}");
        }

        public static TextualVerseModel[] LoadSura(SuraModel sura, EditionConfigModel config)
        {
            var verses = new List<TextualVerseModel>();
            
            string[] arabic = null;
            string[] transliteration = null;
            string[] translation1 = null, translation2 = null, translation3 = null;

            // Pre-load editions
            if (config.AllowArabic)
                arabic = LoadEdition(sura.Number, "ar.uthmani");
            if (config.AllowTransliteration)
                transliteration = LoadEdition(sura.Number, "tr.transliteration");
            if (config.Translation1 != null)
                translation1 = LoadEdition(sura.Number, config.Translation1);
            if (config.Translation2 != null)
                translation2 = LoadEdition(sura.Number, config.Translation2);
            if (config.Translation3 != null)
                translation3 = LoadEdition(sura.Number, config.Translation3);

            // Load verses content
            for (int i = 0; i < sura.Length; i++)
            {
                var verse = new TextualVerseModel()
                {
                    Number = i + 1,
                };
                
                if (config.AllowTransliteration)
                    verse.Transliteration = new VerseEditionModel(true, "tr.transliteration", transliteration[i]);
                if (config.AllowArabic)
                    verse.Arabic = new VerseEditionModel(true, "ar.uthmani", arabic[i]);
                if (config.Translation1 != null)
                    verse.Translation1 = new VerseEditionModel(true, config.Translation1, translation1[i]);
                if (config.Translation2 != null)
                    verse.Translation2 = new VerseEditionModel(true, config.Translation2, translation2[i]);
                if (config.Translation3 != null)
                    verse.Translation3 = new VerseEditionModel(true, config.Translation3, translation3[i]);

                verses.Add(verse);    
            }

            return verses.ToArray();
        }
    }
}