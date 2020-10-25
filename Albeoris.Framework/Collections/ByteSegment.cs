using System;
using System.IO;

namespace Albeoris.Framework.Collections
{
    public sealed class ByteSegment
    {
        public Byte[] Array { get; }
        public Int32 Offset { get; }
        public Int32 Count { get; }

        public ByteSegment(Byte[] array, Int32 offset, Int32 count)
        {
            if (offset > array.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));

            if (count > array.Length - offset)
                throw new ArgumentOutOfRangeException(nameof(count));

            Array = array;
            Offset = offset;
            Count = count;
        }

        public ByteSegment(Byte[] array)
            : this(array, offset: 0, array.Length)
        {
        }

        public ArraySegment<Byte> AsArraySegment() => new ArraySegment<Byte>(Array, Offset, Count);
        public Span<Byte> AsSpan => new Span<Byte>(Array, Offset, Count);
        public Memory<Byte> AsMemory => new Memory<Byte>(Array, Offset, Count);
        public MemoryStream OpenStream => new MemoryStream(Array, Offset, Count);
    }
}