using Baraka.Models;
using Baraka.Models.Quran;
using Baraka.Models.Quran.Configuration;
using Baraka.Services.Quran;
using Baraka.Stores;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
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

        public TextDisplayerViewModel(SelectedSuraStore selectedSuraStore)
        {
            selectedSuraStore.ValueChanged += (newSura) =>
            {
                LoadSura(newSura);
            };
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
