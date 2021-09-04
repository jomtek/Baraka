# Baraka's quran streamer (dev info)
Associated with the player, the verse streamer is an essential part of the software. It gathers and plays verse by verse audio on demand, and, by using the power of the NAudio library, it has :
- An implementation of various control methods such as Seek, Play, Stop
- An audio cache to prevent from overwhelming the online provider
- Real-time data gathering and pre-loading to prevent the user from hearing delay between verses
- Real-time word-by-word highlighting of timestamps called "segments" to help with following the recitation
- Adjustable crossfading to ease the verse transitions


## More about the audio data
- Currently, the data is retrieved verse by verse from [EveryAyah](https://everyayah.com/data/status.php), but, long term, I should buy a VPS to store the data.
- About 15gb of necessary data is hosted on the API
- Data is provided in the MP3 format

Click [here](https://github.com/Jomtek/Baraka/RECITERS.md) for a list of all available reciters...

## Audio cache
The audio cache associates a url with an array of raw MP3 file bytes.

**Internally**
It is managed using a global instance of `AudioCacheManager` located in `Data.LoadedData`. <br>
The instance is built after the audio cache data (stored in `data/cache.ser`) has been deserialized.
After deserialization, the value can be directly passed to the constructor as a `Dictionary<string, byte[]>`.

The `AudioCacheManager`  contains one accessible attribute which is set in the constructor :
 - `Dictionary<string, byte[]> Content { get; private set; }`

As well as three accessible methods :
 - `void AddEntry(string url, byte[] mp3Audio)`
 - `bool HasEntry(string url)`
 - `byte[] Gather(string url)` 

Every time an audio download is requested on `QuranStreamer.DownloadVerseAudio`, if the cache is enabled, a check is done using `HasEntry` to determine if the cache already contains the requested url.
If so, the data is simply retrieved as `byte[]` using `AudioCacheManager.Gather`, and no download is made. Else, the data is downloaded and then added to the cache using `AudioCacheManager.AddEntry`.

**On the GUI**
The audio cache can be enabled or disabled at any time by unchecking "Allow audio cache" in Settings -> General.

## Crossfading


## Word-by-word highlighting