using Baraka.Models;
using Baraka.Models.Quran;
using Baraka.Models.Quran.Configuration;
using Baraka.Services.Quran;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.ViewModels.UserControls.Displayers.TextDisplayer.Design
{
    public class TextDisplayerDesignViewModel : NotifiableBase
    {
        public TextualVerseModel[] Verses
        {
            get
            {
                var config = new EditionConfigModel(false, true, "en.ahmedali", null, null);
                return QuranTextService.LoadSura(SuraInfoService.FromNumber(1), config);
            }
        }

        public TextDisplayerDesignViewModel()
        {
            
        }
    }
}
