using System.Collections.Generic;

namespace YiSha.Util.Helper
{
    public class HtmlHelper
    {
        /// <summary>
        ///     Get part Content from HTML by apply prefix part and subfix part
        /// </summary>
        /// <param name="html">souce html</param>
        /// <param name="prefix">prefix</param>
        /// <param name="subfix">subfix</param>
        /// <returns>part content</returns>
        public static string Resove(string html, string prefix, string subfix)
        {
            var inl = html.IndexOf(prefix);
            if (inl == -1) return null;
            inl += prefix.Length;
            var inl2 = html.IndexOf(subfix, inl);
            var s = html.Substring(inl, inl2 - inl);
            return s;
        }

        public static string ResoveReverse(string html, string subfix, string prefix)
        {
            var inl = html.IndexOf(subfix);
            if (inl == -1) return null;
            var subString = html.Substring(0, inl);
            var inl2 = subString.LastIndexOf(prefix);
            if (inl2 == -1) return null;
            var s = subString.Substring(inl2 + prefix.Length, subString.Length - inl2 - prefix.Length);
            return s;
        }

        public static List<string> ResoveList(string html, string prefix, string subfix)
        {
            var list = new List<string>();
            var index = prefix.Length * -1;
            do
            {
                index = html.IndexOf(prefix, index + prefix.Length);
                if (index == -1) break;
                index += prefix.Length;
                var index4 = html.IndexOf(subfix, index);
                var s78 = html.Substring(index, index4 - index);
                list.Add(s78);
            } while (index > -1);

            return list;
        }
    }
}
