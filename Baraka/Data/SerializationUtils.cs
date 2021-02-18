using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data
{
    public static class SerializationUtils
    {
        public static void Serialize(object sample, string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, sample);
        }

        public static T Deserialize<T>(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(fileStream);
        }
    }
}
