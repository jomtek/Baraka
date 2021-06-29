using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Streaming
{
    class SeekToken
    {
        public bool SeekRequested { get; set; } = false;
        public double Factor { get; set; } = 0;
    }
}
