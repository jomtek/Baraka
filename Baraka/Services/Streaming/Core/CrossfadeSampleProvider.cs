using Baraka.Models.State;
using NAudio.Utils;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;

namespace Baraka.Services.Streaming
{
    /// <summary>
    /// This custom sample provider is a slightly modified version of the MixingSampleProvider.
    /// It gives birth to the crossfading feature which softens the transition between two samples
    /// How it works :
    /// Normally, in the original MixingSampleProvider, a MixerInputEnded event is triggered after the current sample
    /// has finished playing. Then, it is caught by the streamer engine which just plays the next verse.
    /// But, this edited provider acts differently by triggering a PlayNextRequested event just a bit before the sample it
    /// plays has ended (depending on the crossfade amount).
    /// Thus, the next sample/verse is starting to play just milliseconds before the current verse has finished playing, 
    /// and as a result the user hears a beautiful cutless transition.
    /// </summary>
    public class CrossfadeSampleProvider : ISampleProvider
    {
        private readonly List<ISampleProvider> sources;
        private float[] sourceBuffer;
        private const int MaxInputs = 1024; // protect ourselves against doing something silly
        private bool wasPlayNextRequested = false;

        private AppState _app;

        /// <summary>
        /// Creates a new MixingSampleProvider, with no inputs, but a specified WaveFormat
        /// </summary>
        /// <param name="waveFormat">The WaveFormat of this mixer. All inputs must be in this format</param>
        public CrossfadeSampleProvider(WaveFormat waveFormat, AppState app)
        {
            _app = app;

            if (waveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
            {
                throw new ArgumentException("Mixer wave format must be IEEE float");
            }
            sources = new List<ISampleProvider>();
            WaveFormat = waveFormat;
        }

        /// <summary>
        /// When set to true, the Read method always returns the number
        /// of samples requested, even if there are no inputs, or if the
        /// current inputs reach their end. Setting this to true effectively
        /// makes this a never-ending sample provider, so take care if you plan
        /// to write it out to a file.
        /// </summary>
        public bool ReadFully { get; set; }

        public void ClearAudioSources()
        {
            sources.Clear();
        }

        public void AddMixerInput(ISampleProvider mixerInput)
        {
            // we'll just call the lock around add since we are protecting against an AddMixerInput at
            // the same time as a Read, rather than two AddMixerInput calls at the same time
            lock (sources)
            {
                if (sources.Count >= MaxInputs)
                {
                    throw new InvalidOperationException("Too many mixer inputs");
                }
                sources.Add(mixerInput);
            }
            if (WaveFormat == null)
            {
                WaveFormat = mixerInput.WaveFormat;
            }
            else
            {
                if (WaveFormat.SampleRate != mixerInput.WaveFormat.SampleRate ||
                    WaveFormat.Channels != mixerInput.WaveFormat.Channels)
                {
                    throw new ArgumentException("All mixer inputs must have the same WaveFormat");
                }
            }
        }

        /// <summary>
        /// Raised when a mixer input has been removed because it has ended
        /// </summary>
        public event EventHandler<SampleProviderEventArgs> MixerInputEnded;

        /// <summary>
        /// Raised milliseconds before the current mixer input has ended
        /// Creates a crossfaded transition
        /// </summary>
        public event Action PlayNextRequested;

        /// <summary>
        /// The output WaveFormat of this sample provider
        /// </summary>
        public WaveFormat WaveFormat { get; private set; }

        /// <summary>
        /// Reads samples from this sample provider
        /// </summary>
        /// <param name="buffer">Sample buffer</param>
        /// <param name="offset">Offset into sample buffer</param>
        /// <param name="count">Number of samples required</param>
        /// <returns>Number of samples read</returns>
        public int Read(float[] buffer, int offset, int count)
        {
            int outputSamples = 0;
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, count);
            lock (sources)
            {
                int index = sources.Count - 1;
                while (index >= 0)
                {
                    System.Diagnostics.Trace.WriteLine("boucle ---");
                    var source = sources[index];
                    int samplesRead = source.Read(sourceBuffer, 0, count);
                    int outIndex = offset;
                    for (int n = 0; n < samplesRead; n++)
                    {
                        if (n >= outputSamples)
                        {
                            buffer[outIndex++] = sourceBuffer[n];
                        }
                        else
                        {
                            buffer[outIndex++] += sourceBuffer[n];
                        }
                    }
                    outputSamples = Math.Max(samplesRead, outputSamples);
                    
                    // Crossfading logic : trigger PlayNextRequested milliseconds before current sample has finished playing
                    if (source is CachedSoundSampleProvider sampleProvider &&
                        sampleProvider.LengthOfUnplayedData < _app.Settings.CrossfadingValue &&
                        !wasPlayNextRequested)
                    {
                        PlayNextRequested?.Invoke();
                        wasPlayNextRequested = true;
                    } 
                    
                    if (samplesRead < count)
                    {
                        MixerInputEnded?.Invoke(this, new SampleProviderEventArgs(source));
                        sources.RemoveAt(index);
                        wasPlayNextRequested = false;
                    }
                    index--;
                }
            }

            // optionally ensure we return a full buffer
            if (ReadFully && outputSamples < count)
            {
                int outputIndex = offset + outputSamples;
                while (outputIndex < offset + count)
                {
                    buffer[outputIndex++] = 0;
                }
                outputSamples = count;
            }
            return outputSamples;
        }
    }
}