using Baraka.Models.Quran;
using Baraka.Models.State;
using Baraka.Services.Quran;
using Baraka.Services.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Baraka.Commands.UserControls.Displayers.TextDisplayer
{
    public class CursorIncrementCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        private BookmarkState _bookmark;
        private AppState _appState;
        private SoundStreamingService _streamingService;

        public CursorIncrementCommand(BookmarkState bookmark, AppState appState, SoundStreamingService streamingService)
        {
            _bookmark = bookmark;
            _appState = appState;
            _streamingService = streamingService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_bookmark.IsLooping)
            {
                // Check if the loop has finished its cycle
                if (_bookmark.CurrentVerseStore.Value.Number == _bookmark.EndVerseStore.Value)
                {
                    // Re-start the cycle once it's finished
                    _bookmark.CurrentVerseStore.Value = new VerseLocationModel(
                        _bookmark.CurrentVerseStore.Value.Sura,
                        _bookmark.StartVerseStore.Value
                    );
                }
                else
                {
                    _bookmark.CurrentVerseStore.Value = _bookmark.CurrentVerseStore.Value.Next();
                }
            }
            else
            {
                if (_bookmark.CurrentVerseStore.Value.IsLast())
                {
                    _streamingService.Pause();
                }
                else if (_bookmark.CurrentVerseStore.Value.Next().Sura > _appState.SelectedSuraStore.Value.Number)
                {
                    // The case in which the sura has to change
                    var sura = SuraInfoService.FromNumber(_bookmark.CurrentVerseStore.Value.Next().Sura);
                    _appState.SelectedSuraStore.Value = sura;

                    _bookmark.GoToSura(sura);
                    _streamingService.RefreshCursor();
                }
                else
                {
                    _bookmark.CurrentVerseStore.Value = _bookmark.CurrentVerseStore.Value.Next();
                    _bookmark.EndVerseStore.Value = _bookmark.CurrentVerseStore.Value.Number;
                }
            }
        }
    }
}
