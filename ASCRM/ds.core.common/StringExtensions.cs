using System;
using System.Collections.Generic;

namespace ds.core.common
{
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces the multiple values.
        /// </summary>
        /// <param name="orignalString">The orignal string.</param>
        /// <param name="toReplace">To replace.</param>
        /// <returns></returns>
        public static string ReplaceMultipleValues(this string orignalString, List<Tuple<string, string>> toReplace)
        {
            foreach (var replace in toReplace)
            {
                orignalString = orignalString.Replace(replace.Item1, replace.Item2);
            }
            return orignalString;
        }
    }
}
