using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Baraka.Models.Quran.Mushaf
{
    public class MushafPreloadedPageModel
    {
        public int Page { get; private set; }
        public List<StackPanel> Items { get; private set; }

        public MushafPreloadedPageModel(int page, List<StackPanel> items)
        {
            Page = page;
            Items = items;
        }
    }
}
