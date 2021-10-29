using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baraka.Services
{
    static class SearchUtils
    {
        /// <summary>
        /// A method that takes a raw search query given by the user and prepares it. 
        /// </summary>
        /// <param name="rawQuery"></param>
        /// <returns>A trimmed (and possibly lowercased) string</returns>
        public static string PrepareQuery(string rawQuery, bool lowercase = true)
        {
            rawQuery = rawQuery.Trim();
            if (lowercase) rawQuery = rawQuery.ToLowerInvariant();
            return rawQuery;
        }
    }
}
