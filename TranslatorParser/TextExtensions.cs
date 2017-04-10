using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslatorParser
{
    public static class TextExtensions
    {
        public static string FindInnerBlock(this string text, string startChar, string endChar)
        {
            StringBuilder innerBlock = new StringBuilder();
            var start = false;
            foreach (var item in text)
            {
                if (start)
                {
                    innerBlock.Append(item);
                }
                if (item.ToString() == endChar)
                {
                    break;
                }
                if (item.ToString() == startChar)
                {
                    start = true;
                    innerBlock = new StringBuilder();
                    innerBlock.Append(item);
                }
            }
            return innerBlock.ToString();
        }

        public static string FindStartBlock(this string text, string startString, string startChar, string endChar)
        {
            if (text.Contains(startString))
            {
                var index = text.IndexOf(startString);
                var subString = text.Substring(index);
                return subString.FindInnerBlock(startChar, endChar);
            }
            return String.Empty;
        }
    }
}
