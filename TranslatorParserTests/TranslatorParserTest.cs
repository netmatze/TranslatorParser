using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Linq;
using TranslatorParser;
using System.Threading;

namespace TranslatorParserTests
{
    [TestClass]
    public class TranslatorParserTest
    {
        private ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        [TestMethod]
        public void ParserTest()
        {
            Translator translator = new Translator();             
            translator.DownloadCompleted += (translatorObject) => 
            {
                foreach(var item in translatorObject)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var translateCount = value.Values.Where(str => str == "\"übersetzen\"").Count();
                    Assert.AreEqual(translateCount, 1);                    
                    manualResetEvent.Set();
                }
            };
            translator.Translate("übersetzen", "DE", "EN");
            //translator.Translate("translate", "EN", "DE");
            manualResetEvent.WaitOne();
        }
    }
}
