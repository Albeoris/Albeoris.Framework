using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Albeoris.Framework.System;

namespace Albeoris.Framework.Reflection
{
    public sealed class EmbeddedResource
    {
        public ManifestResourceInfo Info { get; }

        public EmbeddedResource(Assembly assembly, String relativePath)
        {
            if (assembly is null) throw new ArgumentNullException(nameof(assembly));
            if (relativePath is null) throw new ArgumentNullException(nameof(relativePath));
            if (relativePath == String.Empty) throw new ArgumentEmptyException(nameof(relativePath));

            String resourceName = FormatResourceName(assembly, relativePath);
            var info = GetManifestResourceInfo(assembly, relativePath, resourceName);
            Info = info;
        }

        private EmbeddedResource(ManifestResourceInfo info)
        {
            Info = info;
        }

        public Stream OpenStream()
        {
            return Info.ReferencedAssembly.GetManifestResourceStream(Info.FileName)
                   ?? throw new InvalidOperationException($"Embedded resource is not available: {Info.FileName}");
        }

        public StreamReader OpenText()
        {
            return new StreamReader(OpenStream());
        }

        public static IEnumerable<EmbeddedResource> Enumerate(Assembly assembly, Wildcard wildcard)
        {
            Wildcard pattern = new Wildcard(FormatResourceName(assembly, wildcard.Pattern), wildcard.StringComparison);
            String[] names = assembly.GetManifestResourceNames();
            foreach (String resourceName in names)
            {
                if (pattern.IsMatch(resourceName))
                {
                    ManifestResourceInfo info = GetManifestResourceInfo(assembly, wildcard.Pattern, resourceName);
                    yield return new EmbeddedResource(info);
                }
            }
        }

        private static ManifestResourceInfo GetManifestResourceInfo(Assembly assembly, String relativePath, String resourceName)
        {
            ManifestResourceInfo info = assembly.GetManifestResourceInfo(resourceName);

            if (info is null)
                throw CreateNotFoundException(assembly, relativePath, resourceName);

            return new ManifestResourceInfo(assembly, resourceName, info.ResourceLocation);
        }

        private static Exception CreateNotFoundException(Assembly assembly, String relativePath, String resourceName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Embedded resource is not exists: {resourceName}");
            sb.AppendLine($"Assembly: {assembly.FullName}");
            sb.AppendLine($"Relative path: {relativePath}");
            sb.AppendLine($"Candidates:");
            foreach (var name in assembly.GetManifestResourceNames())
                sb.Append('\t').AppendLine(name);
            return new FileNotFoundException(sb.ToString());
        }

        private static String FormatResourceName(Assembly assembly, String resourceName)
        {
            String assemblyName = assembly.GetName().Name;
            StringBuilder sb = new StringBuilder(assemblyName.Length + 1 + resourceName.Length);
            sb.Append(assemblyName);
            sb.Append('.');

            String directoryName = Path.GetDirectoryName(resourceName);
            if (directoryName is null)
            {
                sb.Append(resourceName);
                return sb.ToString();
            }

            foreach (var ch in directoryName)
            {
                if (ch == '\\' || ch == '/')
                    sb.Append('.');
                else
                    sb.Append(ch);
            }

            sb.Append('.');
            sb.Append(resourceName.Substring(directoryName.Length + 1));
            return sb.ToString();
        }
    }
}