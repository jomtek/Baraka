﻿using Baraka.Data;
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
                    try
                    {
                        _wout.Stop();
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

        public QuranStreamer()
        {
            // Default values
            Surah = LoadedData.SurahList.ElementAt(0).Key;
            Cheikh = LoadedData.CheikhList.ElementAt(3);

            _wout = new WaveOut(WaveCallbackInfo.FunctionCallback());
        }

        #region Cursor Management
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
        #endregion

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

                var prepareNextAudioTask =
                    new Task(() => DownloadVerseAudio());
                
                if (Verse != Surah.NumberOfVerses)
                {
                    prepareNextAudioTask.Start();
                }

                await Task.Run(PlayNextVerse);

                if (prepareNextAudioTask.Status == TaskStatus.Running)
                {
                    using (new Utils.WaitCursor())
                        prepareNextAudioTask.Wait();
                }

                if (_playing)
                {
                    int currentVerse;
                    if (Utils.General.CheckIfBasmala(Surah))
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

                    //double time = blockAlignedStream.TotalTime.TotalMilliseconds * percentage;
                    //blockAlignedStream.Position =
                    //    (long)(time * _wout.OutputWaveFormat.SampleRate * _wout.OutputWaveFormat.BitsPerSample * _wout.OutputWaveFormat.Channels / 8000.0) & ~1;
                    while (true)
                    {
                        /*if (SeekTok.SeekRequested)
                        { 
                            double time = blockAlignedStream.TotalTime.TotalMilliseconds * SeekTok.Factor;
                            blockAlignedStream.Position =
                                (long)(time *
                                      _wout.OutputWaveFormat.SampleRate *
                                      _wout.OutputWaveFormat.BitsPerSample *
                                      _wout.OutputWaveFormat.Channels / 8000.0) & ~1;

                            SeekTok.SeekRequested = false;    
                        }*/

                        double totalMs = blockAlignedStream.TotalTime.TotalMilliseconds;
                        double currentMs = blockAlignedStream.CurrentTime.TotalMilliseconds;
                        // _cursor = currentMs / totalMs;

                        // Crossfading (WIP TODO)
                        int milliseconds = 1;
                        if (totalMs - currentMs < milliseconds)
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
            }

            //return Task.CompletedTask;
        }
        #endregion
    }
}