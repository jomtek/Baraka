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
        public QuranSamplePlayer(int sampleRate, int channelCount, int id, AppState app)
        {
            // A number to identify the provider
            _id = id;

            // Initialize crossfade mixer
            _mixer = new CrossfadeSampleProvider(
                WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount), app);
            _mixer.ReadFully = true;
            _mixer.PlayNextRequested += Mixer_PlayNextRequested;

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
        }
        #endregion

        #region Events (crossfading logic)
        private void Mixer_PlayNextRequested()
        {
            // Simply pass the event to the service along with current mixer id
            PlayNextRequested?.Invoke(_id);
            
        }
        #endregion

        public void Dispose()
        {
            _outputDevice?.Dispose();
        }
    }
}