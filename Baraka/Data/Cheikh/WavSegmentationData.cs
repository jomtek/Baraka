using Baraka.Data.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Data.Cheikh
{
    [Serializable]
    public class WavSegmentationData
    {
        private Dictionary<(int, int), List<int>> _segments;

        public WavSegmentationData(Dictionary<(int, int), List<int>> data)
        {
            _segments = data;
        }

        public List<int> FindSegments(VerseDescription verse)
        {
            (int, int) key = (verse.Surah.SurahNumber, verse.Number - 1);
            return _segments[key];
        }
    }
}
