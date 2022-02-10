using Baraka.Models.Quran;
using Baraka.Models.Quran.Configuration;
using Baraka.Services.Quran;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Baraka.Models.State;
using System;
using Baraka.Services.Streaming;
using System.Threading;
using System.Windows;

namespace Baraka.ViewModels.UserControls.Displayers.TextDisplayer
{
    public class TextDisplayerViewModel : NotifiableBase
    {
        private BookmarkState _bookmark;
        public BookmarkState Bookmark
        {
            get { return _bookmark; }
            set { _bookmark = value; }
        }

        private AppState _app;
        public AppState App
        {
            get { return _app; }
            set { _app = value; }
        }

        private ObservableCollection<TextualVerseModel> _verses = new();
        public ObservableCollection<TextualVerseModel> Verses
        {
            get { return _verses; }
            set { _verses = value; }
        }

        private double _scrollState = 0;
        public double ScrollState
        {
            get { return _scrollState; }
            set
            {
                if (value != _scrollState)
                {
                    _scrollState = value;
                    OnPropertyChanged(nameof(ScrollState));
                }
            }
        }

        public ICommand SwitchVerseCommand { get; }
        public TextDisplayerViewModel(BookmarkState bookmark, AppState app, SoundStreamingService streamingService)
        {
            Bookmark = bookmark;
            App = app;

            // Load default sura on screen
            LoadSura(App.SelectedSuraStore.Value);

            // Events and commands
            App.SelectedSuraStore.ValueChanged += () =>
            {
                LoadSura(App.SelectedSuraStore.Value);
            };

            streamingService.CursorIncrementRequested += () =>
            {
                if (!_bookmark.CurrentVerseStore.Value.IsLast())
                {
                    // The case in which the sura has to change
                    if (_bookmark.CurrentVerseStore.Value.Next().Sura > _bookmark.CurrentVerseStore.Value.Sura)
                    {
                        var sura = SuraInfoService.FromNumber(_bookmark.CurrentVerseStore.Value.Next().Sura);
                        App.SelectedSuraStore.Value = sura;
                    }

                    _bookmark.CurrentVerseStore.Value = _bookmark.CurrentVerseStore.Value.Next();
                    _bookmark.EndVerseStore.Value = _bookmark.CurrentVerseStore.Value.Number;
                }
                else
                {
                    streamingService.Pause();
                }
            };

            SwitchVerseCommand = new RelayCommand((param) =>
            {
                if (param is TextualVerseModel verse)
                {
                    if (verse.Number < Bookmark.StartVerseStore.Value)
                        Bookmark.StartVerseStore.Value = verse.Number;

                    Bookmark.EndVerseStore.Value = verse.Number;
                    Bookmark.CurrentVerseStore.Value = verse.Location;

                    streamingService.RefreshCursor();
                }
            });
        }

        private void LoadSura(SuraModel sura)
        {
            var config = new EditionConfigModel(true, true, "fr.hamidullah", null, null);

            // TODO: find a way to stop blindly using the dispatcher here (NotSupportedException is thrown)
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                Verses.Clear();
                foreach (var verse in QuranTextService.LoadSura(sura, config))
                    Verses.Add(verse);
            });
        }

        public static TextDisplayerViewModel Create(AppState app, BookmarkState bookmark, SoundStreamingService streamingService)
        {
            return new TextDisplayerViewModel(bookmark, app, streamingService);
        }
    }
}
