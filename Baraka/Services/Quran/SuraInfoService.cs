using Baraka.Models;
using Baraka.Models.Quran;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Services.Quran
{
    public static class SuraInfoService
    {
        private static SuraModel[] _models;
        public static void LoadAll()
        {
            var api = (string)App.Current.FindResource("API_PATH");
            // TODO: adapt to the online api
            var json = File.ReadAllText($@"{api}\text\sura_info.json");
            _models = JsonConvert.DeserializeObject<SuraModel[]>(json);
        }

        public static IEnumerable<SuraModel> GetAll()
        {
            if (_models == null) LoadAll();
            return _models;
        }

        // The number is supposed to start from 1
        public static SuraModel GetByNumber(int number)
        {
            if (number < 1) throw new ArgumentException();
            if (_models == null) LoadAll();
            return _models[number - 1];
        }

        public static IEnumerable<SuraModel> LookUp(string query)
        {
            if (int.TryParse(query, out int number))
            {
                foreach (var sura in _models)
                    if (sura.Number == number)
                        yield return sura;
            }
            else
            {
                foreach (var sura in _models)
                {
                    if (sura.PhoneticName.ToLower().Contains(query))
                    {
                        yield return sura;
                    }
                    else if (sura.TranslatedName.ToLower().Contains(query))
                    {
                        yield return sura;
                    }
                }
            }
        }
    }
}
