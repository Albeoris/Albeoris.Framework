using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
#pragma warning disable 8767

namespace Albeoris.Framework.Collections
{
    public sealed class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> _dic;
        private readonly LinkedList<KeyValuePair<TKey, TValue>> _list;

        public OrderedDictionary(IEqualityComparer<TKey>? compare = null)
        {
            _dic = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>(compare);
            _list = new LinkedList<KeyValuePair<TKey, TValue>>();
        }

        public OrderedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer = null)
            : this(comparer)
        {
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                var node = _list.AddLast(pair);
                _dic.Add(pair.Key, node);
            }
        }

        public Int32 Count => _dic.Count;
        public Boolean IsReadOnly => false;
        public ICollection<TKey> Keys => _list.Select(i => i.Key).ToArray();
        public ICollection<TValue> Values => _list.Select(i => i.Value).ToArray();

        public TValue this[TKey key]
        {
            get => _dic[key].Value.Value;
            set
            {
                if (_dic.TryGetValue(key, out var node))
                {
                    node.Value = new KeyValuePair<TKey, TValue>(key, value);
                }
                else
                {
                    node = _list.AddLast(new KeyValuePair<TKey, TValue>(key, value));
                    _dic[key] = node;
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (_dic.ContainsKey(item.Key))
                _dic.Add(item.Key, default!); // throw exception

            LinkedListNode<KeyValuePair<TKey, TValue>> node = _list.AddLast(item);
            _dic[item.Key] = node;
        }

        public Boolean Remove(TKey key)
        {
            if (_dic.TryGetValue(key, out var node))
            {
                _dic.Remove(key);
                _list.Remove(node);
                return true;
            }

            return false;
        }

        public Boolean Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_dic.TryGetValue(item.Key, out var node) && EqualityComparer<TValue>.Default.Equals(node.Value.Value, item.Value))
            {
                _dic.Remove(item.Key);
                _list.Remove(node);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            _dic.Clear();
            _list.Clear();
        }

        public Boolean ContainsKey(TKey key)
        {
            return _dic.ContainsKey(key);
        }

        public Boolean Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dic.TryGetValue(item.Key, out var node) && EqualityComparer<TValue>.Default.Equals(node.Value.Value, item.Value);
        }

        public Boolean TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (_dic.TryGetValue(key, out var node))
            {
                value = node.Value.Value;
                return true;
            }

            value = default!;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, Int32 arrayIndex)
        {
            if (array is null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex > array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count) throw new ArgumentException(nameof(array));

            Int32 index = arrayIndex;
            foreach (var pair in this)
                array[index++] = pair;
        }
    }
}