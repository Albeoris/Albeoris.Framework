using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Albeoris.Framework.Collections;
using Albeoris.Framework.Exceptions;

namespace Albeoris.Framework.FileSystem
{
    public sealed class FileNameWithoutExtension : IComparable<FileNameWithoutExtension>, IEquatable<FileNameWithoutExtension>
    {
        private readonly String _name;

        public FileNameWithoutExtension(String name)
        {
            _name = name;
        }

        public FileName AddExtension(FileExtension extension) => new FileName(this, extension);

        public static implicit operator String(FileNameWithoutExtension self) => self.ToString();
        public override String ToString() => _name;

        public Int32 CompareTo(FileNameWithoutExtension? other) => OrdinalComparer.Compare(this, other);
        public Boolean Equals(FileNameWithoutExtension? other) => OrdinalComparer.Equals(this, other);
        public override Boolean Equals(Object? obj) => OrdinalComparer.Equals(this, obj as FileNameWithoutExtension);
        public override Int32 GetHashCode() => OrdinalComparer.GetHashCode(this);
        public static Boolean operator ==(FileNameWithoutExtension? left, FileNameWithoutExtension? right) => Equals(left, right);
        public static Boolean operator !=(FileNameWithoutExtension? left, FileNameWithoutExtension? right) => !Equals(left, right);

        public static IProxyComparer<FileNameWithoutExtension?> OrdinalComparer { get; } = MakeComparer(StringComparer.Ordinal);
        public static IProxyComparer<FileNameWithoutExtension?> MakeComparer(StringComparer comparer) => ProxyComparer.CreateComparer((FileNameWithoutExtension? e) => e?._name, comparer);
    }
}