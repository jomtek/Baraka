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
using Baraka.Utils.MVVM;
using Baraka.Commands.UserControls.Displayers.TextDisplayer;

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

        private ICommand _cursorIncrementCommand { get; }

        public ICommand SwitchVerseCommand { get; }
        public ICommand MoveHereCmCommand { get; }
        public ICommand StartHereCmCommand { get; }
        public ICommand DownloadAudioCmCommand { get; }
        public ICommand CopyTextCmCommand { get; }
        public TextDisplayerViewModel(BookmarkState bookmark, AppState app, SoundStreamingService streamingService)
        {
            Bookmark = bookmark;
            App = app;

            // Load default sura on screen
            LoadSura(App.SelectedSuraStore.Value);

            // Commands

            _cursorIncrementCommand = new CursorIncrementCommand(Bookmark, App, streamingService);

            // Verse polygon context-menu commands
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

            StartHereCmCommand = new RelayCommand((param) =>
            {
                if (param is TextualVerseModel verse)
                {
                    Bookmark.StartVerseStore.Value = verse.Location.Number;
                    if (verse.Location.Number > Bookmark.EndVerseStore.Value ||
                        verse.Location.Number > Bookmark.CurrentVerseStore.Value.Number)
                    {
                        if (verse.Location.Number > Bookmark.EndVerseStore.Value)
                        {
                            Bookmark.EndVerseStore.Value = verse.Location.Number;
                        }
                        Bookmark.CurrentVerseStore.Value = verse.Location;
                        streamingService.RefreshCursor();
                    }
                }
            });

            // Events
            App.SelectedSuraStore.ValueChanged += () =>
            {
                LoadSura(App.SelectedSuraStore.Value);
            };

            streamingService.CursorIncrementRequested += () =>
            {
                _cursorIncrementCommand.Execute(null);
            };
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
