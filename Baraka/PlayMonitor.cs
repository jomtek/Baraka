using Baraka.Singletons.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Baraka
{
    public static class PlayMonitor
    {
        private static bool _disposed = false;
        public static Task StartMonitoring()
        {
            while (!_disposed)
            {
                if (!StreamerStateSingleton.Instance.IsPlaying)
                {
                  //  System.Diagnostics.Trace.WriteLine("continuing...");
                  //  Task.Delay(500);
                    continue;
                }

                StreamerEngineSingleton.Instance.PlayVerse(StreamerStateSingleton.Instance.CurrentVerseStore.Value);

                StreamerStateSingleton.Instance.CurrentVerseStore.Value = StreamerStateSingleton.Instance.CurrentVerseStore.Value.Next();
                StreamerStateSingleton.Instance.EndVerseStore.Value++;
                //System.Diagnostics.Trace.WriteLine("playing...");
            }

            return Task.CompletedTask;
        }

        public static void Dispose()
        {
            _disposed = true;
        }
    }
}
