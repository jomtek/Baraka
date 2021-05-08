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
using System.Windows;

namespace Baraka.Streaming
{
    public class QuranStreamer
    {
        private bool _playing = false;
        private bool _loopMode = false;
        private double _cursor = 0;

        public int Verse { get; private set; } = 1;
        public int NonRelativeVerse { get; private set; } = 0;

        public SurahDescription Surah { get; set; }
        public CheikhDescription Cheikh { get; set; }

        public int StartVerse { get; set; } = 0;
        public int EndVerse { get; set; } = -1;

        private WaveOut _wout;
        private byte[] _nextVerseAudio;

        #region Events
        [Category("Baraka")]
        public event EventHandler VerseChanged;

        [Category("Baraka")]
        public event EventHandler FinishedSurah;

        [Category("Baraka")]
        public event EventHandler<double> CursorChanged;
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
                    try
                    {
                        _wout.Stop();
                    } catch (NullReferenceException)
                    {
                        Console.WriteLine("Null reference");
                    }
                }
            }
        }

        public bool LoopMode
        {
            get { return _loopMode; }
            set
            {
                _loopMode = value;

                if (value)
                {
                    // Set the "start verse" before setting LoopMode to true
                    ChangeVerse(StartVerse);
                }
            }
        }

        public double Cursor
        {
            set
            {
                _cursor = value;
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

            NonRelativeVerse = 0;

            Playing = false;
        }

        public async void ChangeVerse(int nonRelativeNum)
        {
            SetVerse(nonRelativeNum);
            NonRelativeVerse = nonRelativeNum;

            if (_playing)
            {
                Playing = false;
                await Task.Delay(50); // Do not precipitate, wait for the stream to stop
                Playing = true;
            }
        }

        private void SetVerse(int number)
        {
            if (Surah.SurahNumber != 1 && Surah.SurahNumber != 9)
            {
                Verse = number;
            }
            else
            {
                Verse = number + 1;
            }
        }

        public QuranStreamer()
        {
            // Default values
            Surah = LoadedData.SurahList.ElementAt(0).Key;
            Cheikh = LoadedData.CheikhList.ElementAt(3);

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
                if (StartVerse > EndVerse) // TODO
                {
                    StartVerse--;
                    continue;
                }

                if (_loopMode && NonRelativeVerse >= EndVerse + 1)
                {            
                    SetVerse(StartVerse);
                    NonRelativeVerse = StartVerse;
                    await Task.Run(() => DownloadVerseAudio(false));
                    continue;
                }

                VerseChanged?.Invoke(this, EventArgs.Empty);

                if (Verse != Surah.NumberOfVerses)
                {
                    new Task(() => DownloadVerseAudio()).Start();
                }
                await Task.Run(PlayNextVerse);
                Console.WriteLine($"done playing {NonRelativeVerse}");

                if (_playing)
                {
                    if (!_loopMode && NonRelativeVerse == Surah.NumberOfVerses - 1)
                    {

                        Reset();
                        FinishedSurah?.Invoke(this, EventArgs.Empty);
                        break;
                    }

                    Verse++;
                    NonRelativeVerse++;
                }
            }
        }

        private async Task PlayNextVerse()
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

               
                    double lastCursor = 0;
                    while (true)
                    {
                        double totalMs = blockAlignedStream.TotalTime.TotalMilliseconds;
                        double currentMs = blockAlignedStream.CurrentTime.TotalMilliseconds;

                        if (_cursor != lastCursor)
                        {
                            // Update cursor
                            //blockAlignedStream.CurrentTime =
                            //    new TimeSpan(Convert.ToInt64(_cursor * blockAlignedStream.TotalTime.Ticks));
                            //blockAlignedStream.Position
                        }
                        else
                        {
                            //_cursor = currentMs / totalMs;
                        }

                        //lastCursor = _cursor;
                        //CursorChanged?.Invoke(this, _cursor);

                        // Crossfading (WIP TODO)
                        int milliseconds = 1;
                        if (totalMs - currentMs < milliseconds)
                        {
                            Console.WriteLine("broke total");
                            break;
                        }

                        if (_wout.PlaybackState == PlaybackState.Stopped)
                        {
                            Console.WriteLine("broke playback");
                            break;
                        }
                        //w.Stop();
                        //Console.WriteLine($"elapsed: {w.ElapsedMilliseconds}");
                        await Task.Delay(10);
                    }

                    _cursor = 0;
                }
            }

            //return Task.CompletedTask;
        }
        #endregion
    }
}