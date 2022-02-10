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

        public event Action CursorIncrementRequested;

        private readonly WebClient _webClient = new();
        private Dictionary<VerseLocationModel, CachedSound> _cache = new();
        private readonly QuranSamplePlayer _soundPlayer1;
        private readonly QuranSamplePlayer _soundPlayer2;

        public List<int> SamplePlayStack = new();
        public bool IsPlaying { get; private set; } = false;

        public SoundStreamingService(
            BookmarkState bookmark, AppState app,
            int sampleRate = 44100, int channelCount = 2)
        {
            _bookmark = bookmark;
            _app = app;

            // We will use two Quran sound providers for crossfading - initialize both of them
            _soundPlayer1 = new QuranSamplePlayer(sampleRate, channelCount, 1, app, this);
            _soundPlayer2 = new QuranSamplePlayer(sampleRate, channelCount, 2, app, this);
            _soundPlayer1.PlayNextRequested += PlayNextRequested;
            _soundPlayer2.PlayNextRequested += PlayNextRequested;
        }

        #region Core (crossfading logic)
        private void PlayNextRequested(int currentMixer)
        {
            CursorIncrementRequested?.Invoke();
            PlayVerse(_bookmark.CurrentVerseStore.Value, currentMixer);
            if (!_bookmark.CurrentVerseStore.Value.IsLast())
                DownloadAndCacheVerse(_bookmark.CurrentVerseStore.Value.Next());
        }
        #endregion

        #region Controls
        public void Pause()
        {
            _soundPlayer1.Pause();
            _soundPlayer2.Pause();
            IsPlaying = false;
        }

        public void Resume()
        {
            _soundPlayer1.Resume();
            _soundPlayer2.Resume();

            IsPlaying = true;

            // If no verse is currently playing, start playing one
            if (SamplePlayStack.Count == 0)
            {
                PlayVerse(_bookmark.CurrentVerseStore.Value);
            }
        }

        public void RefreshCursor()
        {
            _soundPlayer1.ClearAudioSources();
            _soundPlayer2.ClearAudioSources();
            SamplePlayStack.Clear();
            if (IsPlaying) Resume();
        }

        public void ClearCache()
        {
            _cache.Clear();
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
                    _soundPlayer2.PlaySound(_cache[verse]);
                }
                else if (currentMixer == 2)
                {
                    _soundPlayer1.PlaySound(_cache[verse]);
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
                DownloadAndCacheVerse(verse);
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

            // TODO: adapt to the online api
            string api = (string)App.Current.FindResource("API_PATH");

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
            _soundPlayer1.Dispose();
            _soundPlayer2.Dispose();
        }
    }
}