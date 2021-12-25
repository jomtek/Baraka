using Baraka.Models.Quran;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Singletons.Streaming
{
    public class StreamerStateSingleton : NotifiableBase
    {
        private static StreamerStateSingleton _instance;
        public static StreamerStateSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StreamerStateSingleton();
                }
                return _instance;
            }
        }

        public StreamerStateSingleton()
        {
            IsPlaying = false;
            IsLooping = false;
            CurrentVerse = VerseLocationModel.From(AppStateSingleton.Instance.SelectedSuraStore.Value, 1);
            StartVerse = 1;
            EndVerse = 2;
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get { return _isPlaying;; }
            set { _isPlaying = value; OnPropertyChanged(nameof(IsPlaying)); }
        }

        private bool _isLooping;
        public bool IsLooping
        {
            get { return _isLooping; }
            set { _isLooping = value; OnPropertyChanged(nameof(IsLooping)); }
        }

        private VerseLocationModel _currentVerse;
        public VerseLocationModel CurrentVerse
        {
            get { return _currentVerse; }
            set { _currentVerse = value; OnPropertyChanged(nameof(CurrentVerse)); }
        }

        private int _startVerse;
        public int StartVerse
        {
            get { return _startVerse; }
            set { _startVerse = value; OnPropertyChanged(nameof(StartVerse)); }
        }

        private int _endVerse;
        public int EndVerse
        {
            get { return _endVerse; }
            set { _endVerse = value; OnPropertyChanged(nameof(EndVerse)); }
        }
    }
}
