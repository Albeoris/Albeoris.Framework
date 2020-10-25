using System;
using System.Collections.Generic;

namespace Albeoris.Framework.Collections
{
    public sealed class ProxyRelationalComparer<TSource, TTarget> : IComparer<TSource>
    {
        private readonly IComparer<TTarget> _relationalComparer;
        private readonly Func<TSource, TTarget> _selector;

        public ProxyRelationalComparer(IComparer<TTarget> relationalComparer, Func<TSource, TTarget> selector)
        {
            _relationalComparer = relationalComparer;
            _selector = selector;
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