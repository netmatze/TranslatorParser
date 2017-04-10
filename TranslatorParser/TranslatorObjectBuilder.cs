using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslatorParser
{
    public class TranslatorObjectBuilder
    {
        private TranslatorObject mainTranslatorObject;

        public TranslatorObject MainTranslatorObject
        {
            get { return mainTranslatorObject; }
            set { mainTranslatorObject = value; }
        }

        
        private TranslatorObject actualObject;

        public void Create(TranslatorObject translatorObject)
        {
            if (actualObject == null)
            {
                this.actualObject = translatorObject;
                this.mainTranslatorObject = translatorObject;
            }
            else
            {
                translatorObject.ParentTranslatorObject = actualObject;
                this.actualObject.Objects.Add(keyValuePairName, translatorObject);
                this.actualObject = translatorObject;
            }
        }

        private string keyValuePairName = String.Empty;

        public void EndObject()
        {
            this.actualObject = this.actualObject.ParentTranslatorObject;
        }
        
        public void AddValue<T>(T value)
        {            
            this.actualObject.Values.Add(value.ToString());
        }

        public void AddArray()
        {
            this.actualObject.Arrays.Add(new TranslatorArray(this.keyValuePairName));
        }

        public void AddArrayValue(string value)
        {
            TranslatorArray translatorArray = new TranslatorArray(value);
            var array = actualObject.Arrays.ToArray();
            if (array.Length > 0 && array[array.Length - 1] != null)
                array[array.Length - 1].Values.Add(translatorArray);
        }       
    }
}
