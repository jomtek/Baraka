using NAudio.Wave;
using System;

namespace Baraka.Services.Streaming
{
    // https://markheath.net/post/fire-and-forget-audio-playback-with
    class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound _cachedSound;
        public WaveFormat WaveFormat => _cachedSound.WaveFormat;
        
        private long _position;
        public long Position
        {
            get { return _position; }
        }

        public long LengthOfUnplayedData
            => _cachedSound.AudioData.Length - Position;

        public CachedSoundSampleProvider(CachedSound cachedSound)
        {
            this._cachedSound = cachedSound;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = _cachedSound.AudioData.Length - Position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(_cachedSound.AudioData, Position, buffer, offset, samplesToCopy);
            _position += samplesToCopy;
            return (int)samplesToCopy;
        }
    }
}
