using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
