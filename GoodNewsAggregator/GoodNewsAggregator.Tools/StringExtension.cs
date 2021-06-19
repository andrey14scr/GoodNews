using System.Linq;

namespace GoodNewsAggregator.Tools
{
    public static class StringExtension
    {
        public static bool ContainsRole(this string str, string role)
        {
            return str.Split(",").Any(r => r.Trim() == role);
        }
    }
}