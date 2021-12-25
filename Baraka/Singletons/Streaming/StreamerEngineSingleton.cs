using Baraka.Models.Quran;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;

namespace Baraka.Singletons.Streaming
{
    public class StreamerEngineSingleton : IDisposable
    {
        private static StreamerEngineSingleton _instance;
        public static StreamerEngineSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StreamerEngineSingleton();
                }
                return _instance;
            }
        }

        private readonly IWavePlayer _outputDevice;
        private readonly MixingSampleProvider _mixer;
        private Dictionary<VerseLocationModel, CachedSound> _cache = new();

        public StreamerEngineSingleton(int sampleRate = 44100, int channelCount = 2)
        {
            _outputDevice = new WaveOutEvent();
            _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            _mixer.ReadFully = true;
            _outputDevice.Init(_mixer);
            _outputDevice.Play();
            _outputDevice.PlaybackStopped += (object sender, StoppedEventArgs e) =>
            {
                Console.WriteLine();
            };
        }

        #region Core
        private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
        {
            if (input.WaveFormat.Channels == _mixer.WaveFormat.Channels)
            {
                return input;
            }
            else if (input.WaveFormat.Channels == 1 && _mixer.WaveFormat.Channels == 2)
            {
                return new MonoToStereoSampleProvider(input);
            }
            else
            {
                throw new NotImplementedException("Not yet implemented this channel count conversion");
            }
        }
        private void AddMixerInput(ISampleProvider input)
        {
            _mixer.AddMixerInput(ConvertToRightChannelCount(input));
        }

        private void PlaySound(CachedSound sound)
        {
            AddMixerInput(new CachedSoundSampleProvider(sound));
        }
        #endregion

        #region Controls
        public void PlayVerse(VerseLocationModel verse)
        {
            if (_cache.ContainsKey(verse))
            {
                PlaySound(_cache[verse]);
            }
            else
            {
                DownloadVerse(verse);
                PlayVerse(verse);
            }
        }

        public void DownloadVerse(VerseLocationModel verse)
        {
            string api = (string)App.Current.FindResource("API_PATH");
            // TODO: adapt to the online api

            string qari = AppStateSingleton.Instance.SelectedQariStore.Value.Id;
            string fileName =
                verse.Sura.ToString().PadLeft(3, '0') + verse.Number.ToString().PadLeft(3, '0');
            
            var url = $@"{api}\reciters\{qari}\sounds\{fileName}.mp3";
            _cache.Add(verse, new CachedSound(url));
        }
        #endregion

        public void Dispose()
        {
            _outputDevice.Dispose();
        }
    }
}