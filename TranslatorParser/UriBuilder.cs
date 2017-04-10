using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslatorParser
{
    public class UriBuilder
    {
        private string transatorUriString = 
            "http://translate.google.com/translate_a/t?client=t&text={0}&hl={1}&sl={2}&tl={3}&ie=UTF-8&oe=UTF-8&multires=1&otf=1&ssel=0&tsel=0&sc=1";

        public Uri Build(string text, string fromSprache, string toSprache)
        {
            return new Uri(
                string.Format(transatorUriString,
                text, fromSprache, fromSprache, toSprache));
        }
    }
}
