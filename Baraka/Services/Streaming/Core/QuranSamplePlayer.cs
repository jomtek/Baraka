using Baraka.Models.State;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;

namespace Baraka.Services.Streaming.Core
{
    class QuranSamplePlayer : IDisposable
    {
        public event Action<int> PlayNextRequested;

        private readonly IWavePlayer _outputDevice;
        private readonly CrossfadeSampleProvider _mixer;
        private readonly int _id;

        private SoundStreamingService _streamingService;

        public QuranSamplePlayer(int sampleRate, int channelCount, int id, AppState app, SoundStreamingService streamingService)
        {
            _id = id;
            _streamingService = streamingService;

            // Initialize crossfade mixer
            _mixer = new CrossfadeSampleProvider(
                WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount), app);
            _mixer.ReadFully = true;
            _mixer.PlayNextRequested += Mixer_PlayNextRequested;
            _mixer.MixerInputEnded += Mixer_MixerInputEnded;

            // Initialize output device with mixer
            _outputDevice = new WaveOutEvent();
            _outputDevice.Init(_mixer);
            _outputDevice.Play();
        }

        #region Control
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

        public void PlaySound(CachedSound sound)
        {
            var input = new CachedSoundSampleProvider(sound);
            _mixer.AddMixerInput(ConvertToRightChannelCount(input));
            _streamingService.SamplePlayStack.Add(sound.AudioData.Length);
        }

        public void Pause()
        {
            _outputDevice.Pause();
        }

        public void Resume()
        {
            _outputDevice.Play();
        }
        #endregion

        #region Events
        private void Mixer_PlayNextRequested()
        {
            // Simply pass the event to the service along with current mixer id
            PlayNextRequested?.Invoke(_id);
        }

        private void Mixer_MixerInputEnded(object sender, SampleProviderEventArgs e)
        {
            // Pop last sample from play stack
            _streamingService.SamplePlayStack.RemoveAt(_streamingService.SamplePlayStack.Count - 1);
        }
        #endregion

        public void Dispose()
        {
            _outputDevice?.Dispose();
        }
    }
}