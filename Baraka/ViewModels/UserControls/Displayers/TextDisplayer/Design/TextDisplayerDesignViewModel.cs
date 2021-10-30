using Baraka.Models;
using Baraka.Models.Configuration;
using Baraka.Services.Quran;
using Baraka.Utils.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.ViewModels.UserControls.Displayers.TextDisplayer.Design
{
    public class TextDisplayerDesignViewModel : ViewModelBase
    {
        public TextualVerseModel[] Verses
        {
            get
            {
                var config = new EditionConfigModel(true, true, "en.ahmedali", null, null);
                return QuranTextService.LoadSura(SuraInfoService.GetByNumber(1), config);
            }
        }
    }
}
