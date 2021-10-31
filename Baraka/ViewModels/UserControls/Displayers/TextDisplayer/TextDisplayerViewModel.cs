using Baraka.Models;
using Baraka.Models.Configuration;
using Baraka.Services.Quran;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.ViewModels.UserControls.Displayers
{
    public class TextDisplayerViewModel : ViewModelBase
    {
        private ObservableCollection<TextualVerseModel> _verses;
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

        public TextDisplayerViewModel()
        {
            var sura = SuraInfoService.GetByNumber(1);
            var config = new EditionConfigModel(true, true, "fr.hamidullah", null, null);
            Verses =
                new ObservableCollection<TextualVerseModel>(QuranTextService.LoadSura(sura, config));
        }
    }
}
