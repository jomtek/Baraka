using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Utils.Search
{
    public static class General
    {
        public static string PrepareQuery(string query)
        {
            return query.Trim().ToLower();
        }
    }
}
