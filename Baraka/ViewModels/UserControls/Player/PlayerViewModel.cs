﻿using Baraka.Models.Quran;
using Baraka.Models.State;
using Baraka.Services.Quran;
using Baraka.Services.Streaming;
using Baraka.Utils.MVVM;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using Baraka.ViewModels.UserControls.Player.Design;
using Baraka.ViewModels.UserControls.Player.Pages;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Baraka.ViewModels.UserControls.Player
{
    public class PlayerViewModel : NotifiableBase
    {
        private QariTabViewModel _qariTab;
        private SuraTabViewModel _suraTab;
        private UniqueStore<double> _scrollStateStore;
        public Action<bool> PlayerOpenChanged;

        private AppState _app;
        public AppState App
        {
            get { return _app; }
            set { _app = value; }
        }

        private NotifiableBase _currentPage;
        public NotifiableBase CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));

                _scrollStateStore.Value = 0;
            }
        }

        private bool _qariTabSelected = false;
        public bool QariTabSelected
        {
            get { return _qariTabSelected; }
            set
            {
                _qariTabSelected = value;
                OnPropertyChanged(nameof(QariTabSelected));
                
                if (!SuraTabSelected)
                {
                    PlayerOpened = value;
                }

                if (value)
                {
                    SuraTabSelected = false;
                    CurrentPage = _qariTab;
                }
            }
        }

        private bool _suraTabSelected = false;
        public bool SuraTabSelected
        {
            get { return _suraTabSelected; }
            set
            {
                _suraTabSelected = value;
                OnPropertyChanged(nameof(SuraTabSelected));
                
                if (!QariTabSelected)
                {
                    PlayerOpened = value;
                }

                if (value)
                {
                    QariTabSelected = false;
                    CurrentPage = _suraTab;
                }
            }
        }

        private bool _playerOpened = true;
        public bool PlayerOpened
        {
            get { return _playerOpened; }
            set
            {
                _playerOpened = value;
                PlayerOpenChanged?.Invoke(value);
                OnPropertyChanged(nameof(PlayerOpened));
            }
        }

        private double _scrollState;
        public double ScrollState
        {
            get { return _scrollState; }
            set
            {
                _scrollState = value;
                OnPropertyChanged(nameof(ScrollState));
            }
        }

        private bool _loopMode = false;
        public bool Loopmode
        {
            get { return _loopMode; }
            set { _loopMode = value; OnPropertyChanged(nameof(Loopmode)); }
        }

        public ICommand ScrollCommand { get; }
        public ICommand QariTabSelectedCommand { get; }
        public ICommand SuraTabSelectedCommand { get; }
        public ICommand NextSuraCommand { get; }
        public ICommand PreviousSuraCommand { get; }
        public ICommand PlayerPausedCommand { get; }
        public ICommand PlayerResumedCommand { get; }
        public ICommand LoopmodeToggledCommand { get; }
        public PlayerViewModel(AppState app, BookmarkState bookmark, SoundStreamingService streamingService)
        {
            App = app;

            // Stores
            _scrollStateStore = new UniqueStore<double>(0);
            _scrollStateStore.ValueChanged += () =>
            {
                ScrollState = _scrollStateStore.Value;
            };

            App.SelectedQariStore.ValueChanged += () =>
            {
                streamingService.ClearCache();
                streamingService.RefreshCursor();
            };

            // Commands
            ScrollCommand = new RelayCommand((param) =>
                { _scrollStateStore.Value = ScrollState; });

            QariTabSelectedCommand = new RelayCommand((param) =>
                { QariTabSelected = !QariTabSelected; });

            SuraTabSelectedCommand = new RelayCommand((param) =>
                { SuraTabSelected = !SuraTabSelected; });

            NextSuraCommand = new RelayCommand(
                (param) =>
                {
                    var sura = App.SelectedSuraStore.Value.Next();
                    App.SelectedSuraStore.Value = sura;

                    bookmark.GoToSura(sura);
                    streamingService.RefreshCursor();
                },
                (param) =>
                {
                    return App.SelectedSuraStore.Value.Number < 114;
                }
            );

            PreviousSuraCommand = new RelayCommand(
                (param) =>
                {
                    var sura = App.SelectedSuraStore.Value.Last();
                    App.SelectedSuraStore.Value = sura;

                    bookmark.GoToSura(sura);
                    streamingService.RefreshCursor();
                },
                (param) =>
                {
                    return App.SelectedSuraStore.Value.Number > 1;
                }
            );

            PlayerPausedCommand = new RelayCommand((param) =>
            {
                streamingService.Pause();
            });

            PlayerResumedCommand = new RelayCommand((param) =>
            {
                streamingService.Resume();
            });

            LoopmodeToggledCommand = new RelayCommand((param) =>
            {
                Loopmode = !Loopmode;
                bookmark.IsLooping = !bookmark.IsLooping;
            });

            // Tab
            _qariTab = new QariTabViewModel(_scrollStateStore, app);
            _suraTab = new SuraTabViewModel(_scrollStateStore, app, bookmark, streamingService);

            SuraTabSelected = true; // The default tab on the player is the sura tab
        }

        public static PlayerViewModel Create(AppState app, BookmarkState bookmark, SoundStreamingService streamingService)
        {
            return new PlayerViewModel(app, bookmark, streamingService);
        }
    }
}
