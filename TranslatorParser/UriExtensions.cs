using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TranslatorParser
{
    public static class UriExtensions
    {       
        public static void Foreach<T>(this EnumerableRowCollection<T> enumerableRowCollection, Action<T> action)
        {
            foreach (var item in enumerableRowCollection)
            {
                action(item);
            }
        }

        public static IEnumerable<TResult> Foreach<T, TResult>(this EnumerableRowCollection<T> enumerableRowCollection, Func<T, TResult> func)
        {
            foreach (var item in enumerableRowCollection)
            {
                yield return func(item);
            }
        }

        public static void Foreach<T>(this IEnumerable<T> enumerableRowCollection, Action<T> action)
        {
            foreach (var item in enumerableRowCollection)
            {
                action(item);
            }
        }
    }
}
