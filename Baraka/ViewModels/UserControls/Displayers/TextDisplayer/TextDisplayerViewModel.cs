﻿using Baraka.Models;
using Baraka.Models.Configuration;
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

namespace Baraka.ViewModels.UserControls.Displayers.TextDisplayer
{
    public class TextDisplayerViewModel : ViewModelBase
    {
        private ObservableCollection<TextualVerseModel> _verses = new ObservableCollection<TextualVerseModel>();
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

        public TextDisplayerViewModel(SelectedSuraStore selectedSuraStore)
        {
            LoadSura(SuraInfoService.GetByNumber(3));
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
        }
    }
}