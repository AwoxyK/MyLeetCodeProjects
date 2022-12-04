using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NumberToEnglishWords
{
    internal static class StringExtension
    {
        public static string RemoveRepeatingWhitespace(this string str)
        {
            return Regex.Replace(str, @"\s+", " ");
        }
    }
}
