using System;

namespace Immerse
{
    public static class Utils
    {
        public static readonly char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

        public static string CapitilizeFirst(string str)
        {
            str = str[0].ToString().ToUpperInvariant() + str.AsSpan(1).ToString();
            return str;
        }
    }
}
