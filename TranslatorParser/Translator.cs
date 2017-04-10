using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TranslatorParser
{    
    public class Translator
    {
        public readonly string TEXT = "Text";
        public readonly string SUBSTANTIV = "Substantiv";
        public readonly string VERB = "Verb";
        public readonly string OPENSQUAREBRACKET = "[";
        public readonly string CLOSESQUAREBRACKET = "]";

        public event Action<Dictionary<string, TranslatorObject>> DownloadCompleted;

        public Translator()
        {
            DownloadCompleted += (_ => {});
        }

        public void Translate(string text, string fromSprache, string toSprache)
        {
            UriBuilder uriBuilder = new UriBuilder();
            var uri = uriBuilder.Build(text, fromSprache.Substring(0,2), toSprache.Substring(0,2));
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += webClient_DownloadStringCompleted;
            webClient.DownloadStringAsync(uri);            
        }

        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Dictionary<string, TranslatorObject> translatorObjectList = new Dictionary<string, TranslatorObject>();
            var data = ConvertUtf8ToUnicode(e.Result);
            if (data != null)
            {
                string innerText = data.FindInnerBlock(OPENSQUAREBRACKET, CLOSESQUAREBRACKET);
                var substaniveItems = data.FindStartBlock(SUBSTANTIV, OPENSQUAREBRACKET, CLOSESQUAREBRACKET);
                var verbItems = data.FindStartBlock(VERB, OPENSQUAREBRACKET, CLOSESQUAREBRACKET);
                TranslatorObject translatorParserVerbObjects = null;
                TranslatorObject translatorParserSubstantiveObjects = null;
                if (verbItems != String.Empty)
                {
                    translatorParserVerbObjects = TranslatorParserCombinator.Deserialize(verbItems);
                    if (translatorParserVerbObjects.Values.Count > 0)
                    {
                        translatorObjectList.Add(VERB, translatorParserVerbObjects);
                    }
                }
                if (substaniveItems != String.Empty)
                {
                    translatorParserSubstantiveObjects = TranslatorParserCombinator.Deserialize(substaniveItems);
                    if (translatorParserSubstantiveObjects.Values.Count > 0)
                    {
                        translatorObjectList.Add(SUBSTANTIV, translatorParserSubstantiveObjects);
                    }
                }
                if(innerText != String.Empty)
                {
                    var translatorParserObjects = TranslatorParserCombinator.Deserialize(innerText);
                    if(translatorParserObjects.Values.Count > 0)
                    {
                        translatorObjectList.Add(TEXT, translatorParserObjects);
                    }
                }
            }
            DownloadCompleted(translatorObjectList);
        }

        public string ConvertUtf8ToUnicode(string utf8String)
        {
            byte[] utf8Bytes = new byte[utf8String.Length];
            for (int i = 0; i < utf8String.Length; ++i)
            {
                utf8Bytes[i] = (byte)utf8String[i];
            }
            return Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
        }
    }
}
