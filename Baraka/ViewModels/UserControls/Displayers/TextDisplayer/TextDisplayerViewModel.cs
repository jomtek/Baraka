using Baraka.Models;
using Baraka.Models.Quran;
using Baraka.Models.Quran.Configuration;
using Baraka.Services.Quran;
using Baraka.Singletons;
using Baraka.Singletons.Streaming;
using Baraka.Utils.MVVM.Command;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Baraka.ViewModels.UserControls.Displayers.TextDisplayer
{
    public class TextDisplayerViewModel : NotifiableBase
    {
        private ObservableCollection<TextualVerseModel> _verses = new();
        public ObservableCollection<TextualVerseModel> Verses
        {
            get { return _verses; }
            set { _verses = value; }
        }

        private int _capacity;
        public int Capacity
        {
            get { return _capacity; }
            set { _capacity = value; OnPropertyChanged(nameof(Capacity)); }
        }

        private static List<double> _graphicalVersesOffsets = new();
        public static List<double> GraphicalVersesOffsets
        {
            get { return _graphicalVersesOffsets; }
            set { _graphicalVersesOffsets = value; }
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
        public TextDisplayerViewModel()
        {
            AppStateSingleton.Instance.SelectedSuraStore.ValueChanged += () =>
            {
                LoadSura(AppStateSingleton.Instance.SelectedSuraStore.Value);
            };

            SwitchVerseCommand = new RelayCommand((param) =>
            {
                if (param is TextualVerseModel verse)
                {
                    if (verse.Number < StreamerStateSingleton.Instance.StartVerseStore.Value)
                        StreamerStateSingleton.Instance.StartVerseStore.Value = verse.Number;

                    StreamerStateSingleton.Instance.EndVerseStore.Value = verse.Number;
                }
            });
        }

        private void LoadSura(SuraModel sura)
        {
            var config = new EditionConfigModel(true, true, "fr.hamidullah", null, null);
         
            Verses.Clear();
            foreach (var verse in QuranTextService.LoadSura(sura, config))
                Verses.Add(verse);

            Capacity = sura.Length;
        }
    }
}
