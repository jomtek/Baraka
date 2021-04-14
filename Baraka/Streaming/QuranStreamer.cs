using Baraka.Data;
using Baraka.Data.Descriptions;
using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Baraka.Streaming
{
    public class QuranStreamer
    {
        private bool _playing = false;
        public int Verse { get; private set; } = 1;
        public SurahDescription Surah { get; set; }
        public CheikhDescription Cheikh { get; set; }

        private WaveOut _wout;
        private byte[] _nextVerseAudio;

        #region Events
        [Category("Baraka")]
        public event EventHandler VerseChanged;

        [Category("Baraka")]
        public event EventHandler FinishedSurah;
        #endregion

        #region Player Controls
        public bool Playing
        {
            get { return _playing; }
            set
            {
                _playing = value;

                if (value)
                {
                    StartOrResume();
                }
                else
                {
                    _wout.Stop();
                }
            }
        }
        #endregion

        public void Reset()
        {
            if (Surah.SurahNumber != 1 && Surah.SurahNumber != 9)
            {
                Verse = 0;
            }
            else
            {
                Verse = 1;
            }

            Playing = false;
        }

        public async void ChangeVerse(int number)
        {
            Verse = number;
            if (_playing)
            {
                Playing = false;
                await Task.Delay(10); // Do not precipitate, wait for the stream to stop
                Playing = true;
            }
        }

        public QuranStreamer()
        {
            // Default values
            Surah = Data.LoadedData.SurahList.ElementAt(0).Key;
            Cheikh = Data.LoadedData.CheikhList.ElementAt(3);

            _wout = new WaveOut(WaveCallbackInfo.FunctionCallback());
        }

        #region Core
        private string GetCurrentCheikhBasmala()
        {
            return StreamingUtils.GenerateVerseUrl(Cheikh, LoadedData.SurahList.ElementAt(0).Key, 1);
        }

        private void DownloadVerseAudio(bool next = true) // If not next, then actual
        {
            using (MemoryStream ms = new MemoryStream())
            {
                string url;
                if (Verse == 0 && !next)
                {
                    url = GetCurrentCheikhBasmala();
                }
                else
                {
                    url = StreamingUtils.GenerateVerseUrl(Cheikh, Surah, next ? Verse + 1 : Verse);
                }

                using (Stream stream = WebRequest.Create(url)
                    .GetResponse().GetResponseStream())
                {
                    byte[] buffer = new byte[32768];
                    int read;

                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }

                _nextVerseAudio = ms.ToArray();
            }
        }

        private async void StartOrResume()
        {
            await Task.Run(() => DownloadVerseAudio(false));

            while (_playing)
            {
                VerseChanged?.Invoke(this, EventArgs.Empty);

                if (Verse != Surah.NumberOfVerses)
                {
                    new Task(() => DownloadVerseAudio()).Start();
                }
                await Task.Run(PlayNextVerse);

                if (_playing)
                {
                    if (Verse == Surah.NumberOfVerses)
                    {
                        Reset();
                        FinishedSurah?.Invoke(this, EventArgs.Empty);
                        break;
                    }
                }

                if (_playing)
                {
                    Verse++;
                }
            }
        }

        private Task PlayNextVerse()
        {
            using (var ms = new MemoryStream())
            {
                // Write next verse data to memory stream
                ms.Write(_nextVerseAudio, 0, _nextVerseAudio.Length);

                // Convert and play mp3 data
                ms.Position = 0;
                using (WaveStream blockAlignedStream =
                    new BlockAlignReductionStream(
                        WaveFormatConversionStream.CreatePcmStream(
                            new Mp3FileReader(ms))))
                {
                    _wout.Init(blockAlignedStream);
                    try
                    {
                        _wout.Play();
                    }
                    catch (NullReferenceException)
                    {
                        Console.WriteLine("Null reference");
                    };

                    while (_wout.PlaybackState != PlaybackState.Stopped)
                    {
                        int milliseconds = 1;

                        // Crossfading (WIP TODO)
                        if (blockAlignedStream.TotalTime.TotalMilliseconds - blockAlignedStream.CurrentTime.TotalMilliseconds < milliseconds)
                        {
                            break;
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }
        #endregion
    }
}