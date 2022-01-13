using Baraka.Singletons;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Services.Streaming.Core
{
    class QuranSoundProvider : IDisposable
    {
        private readonly IWavePlayer _outputDevice;
        private readonly CrossfadeSampleProvider _mixer;
        private readonly int _number;

        public QuranSoundProvider(int sampleRate, int channelCount, int number)
        {
            // A number to identify the provider
            _number = number;

            // Initialize crossfade mixer
            _mixer = new CrossfadeSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            _mixer.ReadFully = true;
            _mixer.PlayNextRequested += Mixer_PlayNextRequested;

            // Initialize output device with mixer
            _outputDevice = new WaveOutEvent();
            _outputDevice.Init(_mixer);
            _outputDevice.Play();
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

        private void SetCursorToNextVerse()
        {
            StreamerStateSingleton.Instance.CurrentVerseStore.Value =
                StreamerStateSingleton.Instance.CurrentVerseStore.Value.Next();
        }
        #endregion

        #region Control
        public void PlaySound(CachedSound sound)
        {
            var input = new CachedSoundSampleProvider(sound);
            _mixer.AddMixerInput(ConvertToRightChannelCount(input));
        }
        #endregion

        #region Events (crossfading logic)
        private void Mixer_PlayNextRequested()
        {
            // Play next
            SetCursorToNextVerse();
            QuranStreamingService.Instance.PlayVerse(StreamerStateSingleton.Instance.CurrentVerseStore.Value, _number);
            QuranStreamingService.Instance.DownloadAndCacheVerse(StreamerStateSingleton.Instance.CurrentVerseStore.Value.Next());
        }
        #endregion

        public void Dispose()
        {
            _outputDevice?.Dispose();
        }
    }
}
