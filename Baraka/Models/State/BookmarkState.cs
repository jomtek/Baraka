using Baraka.Models.Quran;
using Baraka.Utils.MVVM;
using Baraka.Utils.MVVM.ViewModel;

namespace Baraka.Models.State
{
    public class BookmarkState : NotifiableBase
    {
        private bool _isPlaying = true;
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { _isPlaying = value; OnPropertyChanged(nameof(IsPlaying)); }
        }

        private bool _isLooping = false;
        public bool IsLooping
        {
            get { return _isLooping; }
            set { _isLooping = value; OnPropertyChanged(nameof(IsLooping)); }
        }

        public UniqueStore<int> StartVerseStore { get; set; }
        public UniqueStore<int> EndVerseStore { get; set; }
        public UniqueStore<VerseLocationModel> CurrentVerseStore { get; set; }

        public BookmarkState()
        {}

        public void GoToSura(SuraModel sura)
        {
            StartVerseStore.Value = 1;
            EndVerseStore.Value = 1;
            CurrentVerseStore.Value = new VerseLocationModel(sura.Number, 1);
        }

        public static BookmarkState Create()
        {
            return new BookmarkState()
            {
                IsPlaying = false,
                IsLooping = false,
                StartVerseStore = new UniqueStore<int>(1),
                EndVerseStore = new UniqueStore<int>(1),
                CurrentVerseStore = new UniqueStore<VerseLocationModel>(new VerseLocationModel(1, 1)),
            };
        }

    }
}
