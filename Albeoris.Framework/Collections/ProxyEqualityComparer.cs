using System;
using System.Collections.Generic;

namespace Albeoris.Framework.Collections
{
    public sealed class ProxyEqualityComparer<TSource, TTarget> : IEqualityComparer<TSource>
    {
        private readonly IEqualityComparer<TTarget> _equalityComparer;
        private readonly Func<TSource, TTarget> _selector;

        public ProxyEqualityComparer(IEqualityComparer<TTarget> equalityComparer, Func<TSource, TTarget> selector)
        {
            _equalityComparer = equalityComparer;
            _selector = selector;
        }

        public Boolean Equals(TSource x, TSource y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;

            TTarget xs = _selector(x);
            TTarget ys = _selector(y);

            return _equalityComparer.Equals(xs, ys);
        }

        public Int32 GetHashCode(TSource obj)
        {
            if (ReferenceEquals(obj, null))  return 0;

            return _equalityComparer.GetHashCode(_selector(obj));
        }
    }
}