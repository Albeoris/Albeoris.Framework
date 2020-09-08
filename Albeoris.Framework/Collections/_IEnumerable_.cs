using System.Collections.Generic;

namespace Albeoris.Framework.Collections
{
    // ReSharper disable once InconsistentNaming
    public static class _IEnumerable_
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> self, params T[] values)
        {
            foreach (T item in self)
                yield return item;
            foreach (T item in values)
                yield return item;
        }
    }
}