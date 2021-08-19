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
using System.Windows.Threading;

namespace Baraka.Streaming
{
    public class QuranStreamer
    {
        private bool _playing = false;
        private bool _loopMode = false;

        public int Verse { get; private set; } = 1;
        public int NonRelativeVerse { get; private set; } = 0;

        public SurahDescription Surah { get; set; }
        public CheikhDescription Cheikh { get; set; }

        public int StartVerse { get; set; } = 0;
        public int EndVerse { get; set; } = -1;

        private VerseStreamer _defaultStreamer;
        private VerseStreamer _crossfadingStreamer;
        private byte[] _nextVerseAudio;

        #region Events
        [Category("Baraka")]
        public event EventHandler VerseChanged;

        [Category("Baraka")]
        public event EventHandler FinishedSurah;

        [Category("Baraka")]
        public event EventHandler<int> WordHighlightRequested;
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
                        _defaultStreamer.Stop();
                        _crossfadingStreamer.Stop();
                    }
                    catch (NullReferenceException)
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
        #endregion

        #region Streamer config
        public int OutputDeviceIndex
        {
            set
            {
                _defaultStreamer.SetDevice(value);
                _crossfadingStreamer.SetDevice(value);
                ChangeVerse(NonRelativeVerse);
            }
        }
        #endregion

        public QuranStreamer()
        {
            // Default values
            Surah = LoadedData.SurahList.ElementAt(0).Key;
            Cheikh = LoadedData.CheikhList.ElementAt(3);

            _defaultStreamer = new VerseStreamer();
            _crossfadingStreamer = new VerseStreamer();

            _defaultStreamer.WordHighlightRequested += VerseStreamer_WordHighlightRequested;
            _crossfadingStreamer.WordHighlightRequested += VerseStreamer_WordHighlightRequested;
        }

        #region Cursor Management
        #region Karaoke
        private void VerseStreamer_WordHighlightRequested(object sender, int wordIndex)
        {
            WordHighlightRequested?.Invoke(this, wordIndex);
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

            if (LoopMode)
            {
                EndVerse = Verse;
            }
        }
        #endregion

        #region Core
        private string GetCurrentCheikhBasmala()
        {
            return StreamingUtils.GenerateVerseUrl(Cheikh, Utils.Quran.General.FindSurah(1), 1);
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

                if (LoadedData.Settings.EnableAudioCache && LoadedData.AudioCache.HasEntry(url))
                {
                    _nextVerseAudio = LoadedData.AudioCache.Gather(url);
                }
                else
                {
                    try
                    {
                        // Init request
                        var request = WebRequest.Create(url);
                        request.Timeout = 5000;

                        // Read response
                        using (Stream stream = request.GetResponse().GetResponseStream())
                        {
                            byte[] buffer = new byte[32768];
                            int read;

                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        Playing = false;
                        Utils.Emergency.ShowExMessage(ex);
                        return;
                    }

                    if (LoadedData.Settings.EnableAudioCache)
                    {
                        LoadedData.AudioCache.AddEntry(url, ms.ToArray());
                    }

                    _nextVerseAudio = ms.ToArray();
                }
            }
        }

        private async void StartOrResume()
        {
            await Task.Run(() => DownloadVerseAudio(false));

            while (_playing)
            {
                if (_loopMode && NonRelativeVerse >= EndVerse + 1)
                {            
                    SetVerse(StartVerse);
                    NonRelativeVerse = StartVerse;
                    await Task.Run(() => DownloadVerseAudio(false));
                    continue;
                }

                VerseChanged?.Invoke(this, EventArgs.Empty);

                byte[] audioToPlay = _nextVerseAudio;

                var prepareNextAudioTask =
                    new Task(() => DownloadVerseAudio());
                
                if (Verse != Surah.NumberOfVerses)
                {
                    prepareNextAudioTask.Start();
                }

                try
                {
                    await Task.Run(() => PlayVerse(audioToPlay));
                }
                catch (NAudio.MmException)
                {
                    break;
                }

                if (prepareNextAudioTask.Status == TaskStatus.Running)
                {
                    using (new Utils.WaitCursor())
                        prepareNextAudioTask.Wait();
                }

                if (_playing)
                {
                    int currentVerse;
                    if (Surah.HasBasmala())
                    {
                        currentVerse = NonRelativeVerse - 1;
                    }
                    else
                    {
                        currentVerse = NonRelativeVerse;
                    }

                    if (!_loopMode && currentVerse == Surah.NumberOfVerses - 1)
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

        private bool _lastStreamerWasDefault = false;
        private async Task PlayVerse(byte[] audio)
        {
            if (!_playing)
            {
                return;
            }

            // Explanation: For the sake of crossfading, we want 2 different VerseStreamer instances to play one in two
            VerseStreamer streamer =
                _lastStreamerWasDefault ? _crossfadingStreamer : _defaultStreamer;

            await streamer.Play(audio, Cheikh, new VerseDescription(Surah, Verse));

            _lastStreamerWasDefault = !_lastStreamerWasDefault;
        }
        #endregion
    }
}