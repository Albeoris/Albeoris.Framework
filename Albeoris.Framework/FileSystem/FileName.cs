using System;
using Albeoris.Framework.Collections;
using Albeoris.Framework.Exceptions;
using Albeoris.Framework.Strings;

namespace Albeoris.Framework.FileSystem
{
    public sealed class FileName : IComparable<FileName>, IEquatable<FileName>
    {
        public FileNameWithoutExtension Name { get; }
        public FileExtension Extension { get; }

        public FileName(FileNameWithoutExtension name, FileExtension extension)
        {
            Name = name;
            Extension = extension;
        }

        public FileName ChangeName(FileNameWithoutExtension name) => new FileName(name, Extension);
        public FileName ChangeExtension(FileExtension extension) => new FileName(Name, extension);

        public static implicit operator String(FileName self) => self.ToString();
        public override String ToString() => $"{Name}.{Extension}";

        public Int32 CompareTo(FileName? other) => OrdinalComparer.Compare(this, other);
        public Boolean Equals(FileName? other) => OrdinalComparer.Equals(this, other);
        public override Boolean Equals(Object? obj) => OrdinalComparer.Equals(this, obj as FileName);
        public override Int32 GetHashCode() => OrdinalComparer.GetHashCode(this);
        public static Boolean operator ==(FileName? left, FileName? right) => Equals(left, right);
        public static Boolean operator !=(FileName? left, FileName? right) => !Equals(left, right);

        public static IProxyComparer<FileName?> OrdinalComparer { get; } = MakeComparer(StringComparer.Ordinal);

        public static IProxyComparer<FileName?> MakeComparer(StringComparer comparer)
        {
            IProxyComparer<FileNameWithoutExtension?> name;
            IProxyComparer<FileExtension?> extension;

            if (ReferenceEquals(comparer, StringComparer.Ordinal))
            {
                name = FileNameWithoutExtension.OrdinalComparer;
                extension = FileExtension.OrdinalComparer;
            }
            else
            {
                name = FileNameWithoutExtension.MakeComparer(comparer);
                extension = FileExtension.MakeComparer(comparer);
            }

            return new Comparer(name, extension);
        }
        
        public static FileName Parse(String fileName, FileExtensionType type)
        {
            Int32 dotIndex = type switch
            {
                FileExtensionType.Empty => -1,
                FileExtensionType.SingleDot => fileName.LastIndexOf('.'),
                FileExtensionType.MultiDot => fileName.IndexOf('.'),
                _ => throw Errors.NotSupported(type)
            };

            String name;
            String extension;

            if (dotIndex < 0)
            {
                name = fileName;
                extension = String.Empty;
            }
            else
            {
                name = fileName.Substring(0, dotIndex);
                extension = fileName.Substring(dotIndex + 1);
            }

            return new FileName
            (
                new FileNameWithoutExtension(name),
                new FileExtension(extension)
            );
        }

        private sealed class Comparer : IProxyComparer<FileName?>
        {
            private readonly IProxyComparer<FileNameWithoutExtension> _name;
            private readonly IProxyComparer<FileExtension> _extension;

            public Comparer(IProxyComparer<FileNameWithoutExtension> name, IProxyComparer<FileExtension> extension)
            {
                _name = name;
                _extension = extension;
            }

            public Boolean Equals(FileName? x, FileName? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;

                return _name.Equals(x.Name, y.Name) &&
                       _extension.Equals(x.Extension, y.Extension);
            }

            public Int32 GetHashCode(FileName? obj)
            {
                if (ReferenceEquals(obj, null)) return 0;

                HashCode result = new HashCode();
                result.Add(obj.Name, _name);
                result.Add(obj.Extension, _extension);
                return result.ToHashCode();
            }

            public Int32 Compare(FileName? x, FileName? y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(x, null)) return -1;
                if (ReferenceEquals(y, null)) return 1;

                Int32 name = _name.Compare(x.Name, y.Name);
                if (name != 0) return name;

                return _extension.Compare(x.Extension, y.Extension);
            }
        }
    }
}