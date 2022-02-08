using Baraka.Models.Quran;
using Baraka.Models.Quran.Configuration;
using Baraka.Services.Quran;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Baraka.Models.State;
using System;

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
        public TextDisplayerViewModel(BookmarkState bookmark, AppState app)
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

            SwitchVerseCommand = new RelayCommand((param) =>
            {
                if (param is TextualVerseModel verse)
                {
                    if (verse.Number < Bookmark.StartVerseStore.Value)
                        Bookmark.StartVerseStore.Value = verse.Number;

                    Bookmark.EndVerseStore.Value = verse.Number;
                    Bookmark.CurrentVerseStore.Value = verse.Location;
                }
            });
        }

        private void LoadSura(SuraModel sura)
        {
            var config = new EditionConfigModel(true, true, "fr.hamidullah", null, null);
         
            Verses.Clear();
            foreach (var verse in QuranTextService.LoadSura(sura, config))
                Verses.Add(verse);
        }

        public static TextDisplayerViewModel Create(AppState app, BookmarkState bookmark)
        {
            return new TextDisplayerViewModel(bookmark, app);
        }
    }
}
