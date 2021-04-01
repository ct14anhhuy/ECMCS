using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ECMCS.Utilities
{
    public static class UrlHelper
    {
        public static string Encode(string str)
        {
            var charClass = string.Format("0-9a-zA-Z{0}", Regex.Escape("-_.!~*'()"));
            return Regex.Replace(str, string.Format("[^{0}]", charClass), new MatchEvaluator(EncodeEvaluator));
        }

        private static string EncodeEvaluator(Match match)
        {
            return (match.Value == " ") ? "+" : string.Format("%{0:X2}", Convert.ToInt32(match.Value[0]));
        }

        private static string DecodeEvaluator(Match match)
        {
            return Convert.ToChar(int.Parse(match.Value.Substring(1), NumberStyles.HexNumber)).ToString();
        }

        public static string Decode(string str)
        {
            return Regex.Replace(str, "%[0-9a-zA-Z고객센터][0-9a-zA-Z고객센터]", new MatchEvaluator(DecodeEvaluator));
        }
    }
}