using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baraka.Data;
using NAudio;
using NAudio.Wave;

namespace Baraka.Streaming
{
    public class VerseStreamer
    {
        private WaveOut _wout;
        private WaveStream _wStream;
        private MemoryStream _mStream;

        public VerseStreamer()
        {
            _wout = new WaveOut(WaveCallbackInfo.FunctionCallback());
            _wStream = null;
            _mStream = new MemoryStream();
        }

        #region Controller
        public async Task Play(byte[] audio)
        {
            _mStream.SetLength(0); // Clear the stream
            _mStream.Write(audio, 0, audio.Length); // Write audio
            _mStream.Position = 0;

            if (_wStream != null) _wStream.Dispose();
            _wStream = new BlockAlignReductionStream(
                        WaveFormatConversionStream.CreatePcmStream(
                            new Mp3FileReader(_mStream)));

            try
            {
                _wout.Init(_wStream);
            }
            catch (MmException ex)
            {
                throw ex;
            }

            _wout.Play();

            while (true)
            {
                double totalMs = _wStream.TotalTime.TotalMilliseconds;
                double currentMs = _wStream.CurrentTime.TotalMilliseconds;

                // Crossfading (WIP TODO)
                if (totalMs - currentMs < LoadedData.Settings.CrossFadingValue)
                {
                    break;
                }

                if (_wout.PlaybackState == PlaybackState.Stopped)
                {
                    break;
                }

                await Task.Delay(10);
            }
        }
        public void Stop()
        {
            _wout.Stop();
        }

        public void SetDevice(int number)
        {
            _wout.DeviceNumber = number;
        }
        #endregion
    }
}
