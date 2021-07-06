using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Utils
{
    public static class Audio
    {
        public static void Combine(List<byte[]> parts, string outputFile)
        {
            using (var ms = new MemoryStream())
            {
                foreach (byte[] part in parts)
                {
                    MemoryStream stream = new MemoryStream(part);

                    Mp3FileReader reader = new Mp3FileReader(stream);
                    if ((ms.Position == 0) && (reader.Id3v2Tag != null))
                    {
                        ms.Write(reader.Id3v2Tag.RawData, 0, reader.Id3v2Tag.RawData.Length);
                    }

                    Mp3Frame frame;
                    while ((frame = reader.ReadNextFrame()) != null)
                    {
                        ms.Write(frame.RawData, 0, frame.RawData.Length);
                    }
                }

                using (var fs = new FileStream(outputFile, FileMode.Create))
                {
                    ms.Position = 0;
                    ms.CopyTo(fs);
                    fs.Flush();
                }
            }
        }


    }
}
