using Baraka.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public static SuraModel[] GetAll()
        {
            if (_models == null) LoadAll();
            return _models;
        }
    }
}
