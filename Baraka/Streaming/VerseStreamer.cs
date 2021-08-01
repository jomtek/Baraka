using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baraka.Data;
using Baraka.Data.Descriptions;
using NAudio;
using NAudio.Wave;

namespace Baraka.Streaming
{
    public class VerseStreamer
    {
        private WaveOut _wout;
        private WaveStream _wStream;
        private MemoryStream _mStream;

        private Stopwatch _stopWatch;

        #region Events
        [Category("Baraka")]
        public event EventHandler<int> WordHighlightRequested;
        #endregion

        public VerseStreamer()
        {
            _wout = new WaveOut(WaveCallbackInfo.FunctionCallback());
            _wStream = null;
            _mStream = new MemoryStream();

            _stopWatch = new Stopwatch();
        }

        #region Controller
        private bool CheckCrossfading()
        {
            double cfBytesLimit =
                (LoadedData.Settings.CrossFadingValue / 1000d) * _wStream.WaveFormat.AverageBytesPerSecond;
            return _wStream.Length - _wStream.Position < cfBytesLimit;
        }

        public async Task Play(byte[] audio, CheikhDescription cheikh, VerseDescription verse)
        {
            List<int> segments = null;
            if (verse.Number == 0)
            {
                var fatihaFirstVerse = new VerseDescription(LoadedData.SurahList.ElementAt(0).Key, 2);
                segments = cheikh.WavSegments.FindSegments(fatihaFirstVerse);
            }
            else if (cheikh.WavSegments != null)
            {
                segments = cheikh.WavSegments.FindSegments(verse);
            }

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
            _stopWatch.Restart();

            int lastSelectedWord = -1;
            while (_wout.PlaybackState != PlaybackState.Stopped)
            {
                if (CheckCrossfading())
                {
                    break;
                }

                if (segments != null) // temp
                {
                    long currentTime = Utils.Audio.RoundOff(_stopWatch.ElapsedMilliseconds);

                    // TODO: any idea on how to optimize this bit of code ?
                    // (perhaps use a variable where to save the last used segment?)
                    for (int i = 0; i < segments.Count; i++)
                    {
                        if (currentTime < segments[i])
                        {
                            if (i != lastSelectedWord)
                            {
                                Console.WriteLine($"requesting highlight on verse {verse.Number}");
                                // Highlight i_th word on the current verse
                                App.Current.Dispatcher.Invoke(new Action(() => WordHighlightRequested?.Invoke(this, i)));
                                lastSelectedWord = i;
                            }
                            break;
                        }
                    }
                }

                await Task.Delay(10);
            }

            // Clear highlighting on the verse
            App.Current.Dispatcher.Invoke(new Action(() => WordHighlightRequested?.Invoke(this, -1)));
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
