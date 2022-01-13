using Baraka.Models.Quran;
using Baraka.Services.Streaming.Core;
using Baraka.Singletons;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Net;

namespace Baraka.Services.Streaming
{
    public class QuranStreamingService : IDisposable
    {
        private static QuranStreamingService _instance;
        public static QuranStreamingService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new QuranStreamingService();
                }
                return _instance;
            }
        }

        private readonly WebClient _webClient = new();
        private Dictionary<VerseLocationModel, CachedSound> _cache = new();
        private readonly QuranSoundProvider _soundProvider1;
        private readonly QuranSoundProvider _soundProvider2;
        public QuranStreamingService(int sampleRate = 44100, int channelCount = 2)
        {
            // We will use two Quran sound providers for crossfading - initialize both of them
            _soundProvider1 = new QuranSoundProvider(sampleRate, channelCount, 1);
            _soundProvider2 = new QuranSoundProvider(sampleRate, channelCount, 2);
        }

        #region Controls
        public void PlayVerse(VerseLocationModel verse, int currentMixer)
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
                DownloadAndCacheVerse(verse);
                PlayVerse(verse, currentMixer);
            }
        }

        public void DownloadAndCacheVerse(VerseLocationModel verse)
        {
            string api = (string)App.Current.FindResource("API_PATH");
            // TODO: adapt to the online api

            string qari = AppStateSingleton.Instance.SelectedQariStore.Value.Id;
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