using Baraka.Models.Quran;
using Baraka.Models.State;
using Baraka.Services.Streaming.Core;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Baraka.Services.Streaming
{
    // Huge thanks to 'Mark Heath'
    public class SoundStreamingService : IDisposable
    {
        private BookmarkState _bookmark;
        private AppState _app;

        private readonly WebClient _webClient = new();
        private Dictionary<VerseLocationModel, CachedSound> _cache = new();
        private readonly QuranSamplePlayer _soundProvider1;
        private readonly QuranSamplePlayer _soundProvider2;

        public List<int> SamplePlayStack = new();

        public SoundStreamingService(
            BookmarkState bookmark, AppState app,
            int sampleRate = 44100, int channelCount = 2)
        {
            _bookmark = bookmark;
            _app = app;

            // We will use two Quran sound providers for crossfading - initialize both of them
            _soundProvider1 = new QuranSamplePlayer(sampleRate, channelCount, 1, app, this);
            _soundProvider2 = new QuranSamplePlayer(sampleRate, channelCount, 2, app, this);
            _soundProvider1.PlayNextRequested += PlayNextRequested;
            _soundProvider2.PlayNextRequested += PlayNextRequested;
        }

        #region Core (crossfading logic)
        private void SetCursorToNextVerse()
        {
            _bookmark.CurrentVerseStore.Value = _bookmark.CurrentVerseStore.Value.Next();
        }

        private void PlayNextRequested(int currentMixer)
        {
            SetCursorToNextVerse();
            PlayVerse(_bookmark.CurrentVerseStore.Value, currentMixer);
            DownloadAndCacheVerse(_bookmark.CurrentVerseStore.Value.Next());
        }
        #endregion

        #region Controls
        public void Pause()
        {
            _soundProvider1.Pause();
            _soundProvider2.Pause();
            Trace.WriteLine("paused!");
        }

        public void Resume()
        {
            _soundProvider1.Resume();
            _soundProvider2.Resume();

            // If no verse is currently playing, start playing one
            if (SamplePlayStack.Count == 0)
            {
                PlayVerse(_bookmark.CurrentVerseStore.Value);
            }

            Trace.WriteLine("resumed!");
        }

        public void PlayVerse(VerseLocationModel verse, int currentMixer = 1)
        {
            if (_cache.ContainsKey(verse))
            {
                /*
                Alternate between mixer 1, then 2, then 1, with
                short cuts, in order to create a crossfaded transition
                */
                if (currentMixer == 1)
                {
                    _soundProvider2.PlaySound(_cache[verse]);
                }
                else if (currentMixer == 2)
                {
                    _soundProvider1.PlaySound(_cache[verse]);
                }
                else
                {
                    throw new ArgumentException("currentMixer should be 1 or 2");
                }
            }
            else
            {
                /*
                Manage the first verse by first downloading it in order
                to start the play loop
                */
                Trace.WriteLine("downloading verse...");
                DownloadAndCacheVerse(verse);
                Trace.WriteLine("...done!");
                PlayVerse(verse, currentMixer);
            }
        }

        public void DownloadAndCacheVerse(VerseLocationModel verse)
        {
            if (_cache.ContainsKey(verse))
                return;

            // If cache gets too big, just remove the oldest element to save space
            if (_cache.Keys.Count > _app.Settings.AudioCacheLength)
                _cache.Remove(_cache.Keys.First());

            string api = (string)App.Current.FindResource("API_PATH");
            // TODO: adapt to the online api

            string qari = _app.SelectedQariStore.Value.Id;
            string fileName =
                verse.Sura.ToString().PadLeft(3, '0') + verse.Number.ToString().PadLeft(3, '0');

            string url = $@"{api}\reciters\{qari}\sounds\{fileName}.mp3";
            byte[] data = _webClient.DownloadData(url);
            _cache.Add(verse, new CachedSound(data));
        }
        #endregion

        public void Dispose()
        {
            _soundProvider1.Dispose();
            _soundProvider2.Dispose();
        }
    }
}