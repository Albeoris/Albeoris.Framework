using System;
using System.Collections.Generic;
using Albeoris.Framework.FileSystem;

namespace Albeoris.Framework.Collections
{
    public static class ProxyComparer
    {
        public static IProxyComparer<TSource> CreateComparer<TSource, TTarget, TComparer>(Func<TSource, TTarget> selector, TComparer comparer)
            where TComparer : IEqualityComparer<TTarget>, IComparer<TTarget>
        {
            return new ProxyComparer<TSource, TTarget>(comparer, comparer, selector);
        }
    }

    public sealed class ProxyComparer<TSource, TTarget> : IProxyComparer<TSource>
    {
        private readonly IEqualityComparer<TTarget> _equalityComparer;
        private readonly IComparer<TTarget> _relationalComparer;
        private readonly Func<TSource, TTarget> _selector;

        public ProxyComparer(IEqualityComparer<TTarget> equalityComparer, IComparer<TTarget> relationalComparer, Func<TSource, TTarget> selector)
        {
            _equalityComparer = equalityComparer;
            _relationalComparer = relationalComparer;
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

        public Int32 Compare(TSource x, TSource y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(x, null)) return -1;
            if (ReferenceEquals(y, null)) return 1;

            TTarget xs = _selector(x);
            TTarget ys = _selector(y);

            return _relationalComparer.Compare(xs, ys);
        }
    }
}