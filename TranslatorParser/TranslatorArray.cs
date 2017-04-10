using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslatorParser
{
    public class TranslatorArray : IValue
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private List<IValue> values = new List<IValue>();

        public List<IValue> Values
        {
            get 
            {                
                return values; 
            }
            set 
            { 
                values = value; 
            }
        }

        public TranslatorArray(params IValue[] arrayValues)
        {            
            foreach (var value in arrayValues)
            {
                values.Add(value);
            }
        }

        public TranslatorArray(string name, params IValue[] arrayValues) : this(arrayValues)
        {
            this.name = name;            
        }
    }
}
