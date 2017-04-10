using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslatorParser
{
    public class TranslatorObject
    {
        private TranslatorObject parentTranslatorObject;

        public TranslatorObject ParentTranslatorObject
        {
            get { return parentTranslatorObject; }
            set { parentTranslatorObject = value; }
        }

        private List<TranslatorArray> arrays = new List<TranslatorArray>();

        public List<TranslatorArray> Arrays
        {
            get { return arrays; }
            set { arrays = value; }
        }

        private List<string> values = new List<string>();

        public List<string> Values
        {
            get { return values; }
            set { values = value; }
        }

        private Dictionary<string, TranslatorObject> objects = new Dictionary<string, TranslatorObject>();

        public Dictionary<string, TranslatorObject> Objects
        {
            get { return objects; }
            set { objects = value; }
        }
    }
}
