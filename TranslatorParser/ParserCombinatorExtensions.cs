using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslatorParser
{
    public static class ParserCombinatorsExtensions
    {       
        public static Parse<T2> Bind<T1, T2>(this Parse<T1> parse1,
           Func<T1, Parse<T2>> parse2)
        {
            return value =>
            {
                var parse1Result = parse1(value);
                return parse2(parse1Result.Result)(parse1Result.RemainingInput);
            };
        }
    }
}
