using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Theme.UserControls.Quran.Display
{
    public interface ISurahDisplayer
    {
        Task LoadSurahAsync(SurahDescription surah);
        void UnloadActualSurah();
    }
}
