# Baraka's quran streamer (dev info)
Associated with the player, the verse streamer is an essential part of the software. It gathers and plays verse by verse audio on demand, and, by using the power of the [NAudio](https://github.com/naudio/NAudio) library, it has :
- An implementation of various control methods such as seek, play, stop...
- An audio cache to prevent from overwhelming the online provider
- Real-time data gathering and pre-loading to prevent the user from hearing delay between verses
- Real-time word-by-word highlighting of timestamps called "segments" to help with following the recitation
- Adjustable crossfading to ease the verse transitions

Summary
 1. More about the audio data
 2. Audio cache
 3. Cross fading
 4. IA-powered word-by-word highlighting

## 1. More about the audio data
- Currently, the data is retrieved verse by verse from [EveryAyah](https://everyayah.com/data/status.php), but, long term, I should buy a VPS to store the data.
- About 15gb of necessary data is hosted on the API
- Data is provided in the MP3 format

Click [here](https://github.com/Jomtek/Baraka/RECITERS.md) for a list of all available reciters...

## 2. Audio cache
The audio cache associates a url with an array of raw MP3 file bytes.

**Internally**<br>
It is managed using a global instance of `AudioCacheManager` located in `Data.LoadedData`. <br>
The instance is built after the audio cache data (stored in `data/cache.ser`) has been deserialized.
After deserialization, the value can be directly passed to the constructor as a `Dictionary<string, byte[]>`.

The `AudioCacheManager`  contains one accessible attribute which is set in the constructor :
 - `Dictionary<string, byte[]> Content { get; private set; }`

As well as three accessible methods :
 - `void AddEntry(string url, byte[] mp3Audio)`
 - `bool HasEntry(string url)`
 - `byte[] Gather(string url)` 

Every time an audio download is requested on `QuranStreamer.DownloadVerseAudio`, if the cache is enabled, a check is done using `HasEntry` to determine if the cache already contains the requested url.<br>
If so, the data is simply retrieved as `byte[]` using `AudioCacheManager.Gather`, and no download is made. Else, the data is downloaded and then added to the cache using `AudioCacheManager.AddEntry`.<br>

**On the GUI**<br>
The audio cache can be enabled or disabled at any time by unchecking "Allow audio cache" in Settings -> General.

## 3. Crossfading
The crossfading feature eases the transitions between verses by preventing unwanted cuts.

**Internally**<br>
Baraka's crossfading is implemented using two separate instances of `VerseStreamer`, alternating 1/2 each.<br>

The `PlayVerse` method located in `QuranStreamer` **awaits** the `VerseStreamer.Play` task.<br> 

Each `VerseStreamer` continuously tries to deduce when to stop playing using its `CheckCrossfading` method. and, thus, unblocks the `PlayVerse` method just   milliseconds before the current verse has finished playing.<br>

        private bool CheckCrossfading()
        {
            double cfBytesLimit =
                (LoadedData.Settings.CrossFadingValue / 1000d) * _wStream.WaveFormat.AverageBytesPerSecond;
            return _wStream.Length - _wStream.Position < cfBytesLimit;
        }

<br>This creates a smooth unnoticeable transition to the next (and previous) `VerseStreamer` instance, now playing the next verse.

**On the GUI**<br>
The crossfading intensity can changed any time by tweaking "Crossfading value (ms)"  in Settings -> Reading -> [Audio].

## 4. IA-powered word-by-word highlighting