using Baraka.Models.Quran;
using Baraka.Utils.MVVM;
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
            CurrentVerseStore = new UniqueStore<VerseLocationModel>(VerseLocationModel.From(AppStateSingleton.Instance.SelectedSuraStore.Value, 1));
            StartVerseStore = new UniqueStore<int>(1);
            EndVerseStore = new UniqueStore<int>(1);
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { _isPlaying = value; OnPropertyChanged(nameof(IsPlaying)); }
        }

        private bool _isLooping;
        public bool IsLooping
        {
            get { return _isLooping; }
            set { _isLooping = value; OnPropertyChanged(nameof(IsLooping)); }
        }

        public UniqueStore<int> StartVerseStore { get; }
        public UniqueStore<int> EndVerseStore { get; }
        public UniqueStore<VerseLocationModel> CurrentVerseStore { get; }
    }
}
