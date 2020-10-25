using System.Collections.Generic;

namespace Albeoris.Framework.Collections
{
    public interface IProxyComparer<in T> : IEqualityComparer<T>, IComparer<T>
    {
    }
}