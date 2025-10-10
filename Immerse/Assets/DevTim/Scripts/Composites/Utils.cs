using System;

namespace Immerse
{
    public static class Utils
    {
        public static string CapitilizeFirst(string str)
        {
            str = str[0].ToString().ToUpperInvariant() + str.AsSpan(1).ToString();
            return str;
        }
    }
}
