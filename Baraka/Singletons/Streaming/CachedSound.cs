using NAudio.Wave;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Baraka.Singletons.Streaming
{
    public class CachedSound
    {
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }
        public CachedSound(string url)
        {
            using (var ms = new MemoryStream(new WebClient().DownloadData(url)))
            using (var audioFileReader = new AudioStreamReader(ms))
            {
                WaveFormat = audioFileReader.WaveFormat;
                var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
                var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                }
                AudioData = wholeFile.ToArray();
            }
        }
    }
}
