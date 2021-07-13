using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data
{
    public class AudioCacheManager
    {
        public Dictionary<string, byte[]> Content { get; private set; }

        public AudioCacheManager(Dictionary<string, byte[]> content)
        {
            Content = content;
        }

        public void AddEntry(string url, byte[] mp3Audio)
        {
            string key = Utils.General.CreateMD5(url);
            Content.Add(key, mp3Audio);
        }

        public bool HasEntry(string url)
        {
            string key = Utils.General.CreateMD5(url);
            return Content.ContainsKey(key);
        }

        public byte[] Gather(string url)
        {
            string key = Utils.General.CreateMD5(url);
            return Content[key];
        }

        public void Clear()
        {
            Content.Clear();
        }
    }
}
