using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ECMCS.Utilities
{
    public static class StringHelper
    {
        public static string[] Extract(this string source, string start, string end)
        {
            List<string> str = new List<string>();
            string pattern = string.Format("{0}({1}){2}", Regex.Escape(start), ".+?", Regex.Escape(end));
            foreach (Match m in Regex.Matches(source, pattern))
            {
                str.Add(m.Groups[1].Value.Trim());
            }
            return str.ToArray();
        }
    }
}