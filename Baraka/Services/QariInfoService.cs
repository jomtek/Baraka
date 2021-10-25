using Baraka.Models;
using Newtonsoft.Json;
using System.IO;

namespace Baraka.Services
{
    public static class QariInfoService
    {
        private static QariModel[] _models;
        public static void LoadAll()
        {
            var api = (string)App.Current.FindResource("API_PATH");
            // TODO: adapt to the online api
            var json = File.ReadAllText($@"{api}\reciters\info.json");
            _models = JsonConvert.DeserializeObject<QariModel[]>(json);
        }

        public static QariModel[] GetAll()
        {
            if (_models == null) LoadAll();
            return _models;
        }
    }
}
