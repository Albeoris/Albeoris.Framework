using System;
using Albeoris.Framework.Collections;

namespace Albeoris.Framework.FileSystem
{
    public sealed class FileExtension : IComparable<FileExtension>, IEquatable<FileExtension>
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public static Boolean DisableIntern { get; set; }
        
        private readonly String _extension;

        public FileExtension(String extension)
        {
            _extension = DisableIntern
                ? extension
                : String.Intern(extension);
        }

        public static implicit operator String(FileExtension self) => self.ToString();
        public override String ToString() => _extension;

        public Int32 CompareTo(FileExtension? other) => OrdinalComparer.Compare(this, other);
        public Boolean Equals(FileExtension? other) => OrdinalComparer.Equals(this, other);
        public override Boolean Equals(Object? obj) => OrdinalComparer.Equals(this, obj as FileExtension);
        public override Int32 GetHashCode() => OrdinalComparer.GetHashCode(this);
        public static Boolean operator ==(FileExtension? left, FileExtension? right) => Equals(left, right);
        public static Boolean operator !=(FileExtension? left, FileExtension? right) => !Equals(left, right);

        public static IProxyComparer<FileExtension?> OrdinalComparer { get; } = MakeComparer(StringComparer.Ordinal);
        public static IProxyComparer<FileExtension?> MakeComparer(StringComparer comparer) => ProxyComparer.CreateComparer((FileExtension? e) => e?._extension, comparer);
    }
}