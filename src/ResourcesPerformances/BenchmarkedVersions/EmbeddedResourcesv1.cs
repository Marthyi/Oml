using System;
using System.IO;
using System.Reflection;

namespace Oml.Resources
{
    public static class EmbeddedResourcesv1
    {
        /// <summary>
        /// Open a read stream on embedded resource
        /// </summary>
        /// <param name="assembly">Assembly containing file</param>
        /// <param name="resourcePath">path to the resource</param>
        /// <returns></returns>
        public static Stream ReadAsStream(this Assembly assembly, string resourcePath)
        {
            assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            resourcePath = resourcePath ?? throw new ArgumentNullException(nameof(resourcePath));

            resourcePath = resourcePath.Replace('/', '.');
            resourcePath = resourcePath.Replace('\\', '.');

            resourcePath = resourcePath switch
            {
                string p when string.IsNullOrWhiteSpace(p) => throw new ArgumentException($"Resource with location '{p}' is invalid"),
                string p when p.StartsWith('.') && p.Length == 1 => throw new ArgumentException($"Resource with location '{p}' is invalid"),
                string p when p.StartsWith('.') => p[1..],
                string p => resourcePath
            };

            resourcePath = $"{assembly.GetName().Name}.{resourcePath}";

            var info = assembly.GetManifestResourceInfo(resourcePath);

            if (info == null)
            {
                throw new ArgumentException($"Resource with location '{resourcePath}' does not exists");
            }

            return assembly.GetManifestResourceStream(resourcePath);
        }

        /// <summary>
        /// Open a read stream on embedded resource
        /// </summary>
        /// <param name="assembly">Assembly containing file</param>
        /// <param name="resourcePath">path to the resource</param>
        /// <returns></returns>
        public static string ReadAsText(this Assembly assembly, string resourcePath)
        {
            using StreamReader sr = new StreamReader(ReadAsStream(assembly, resourcePath));
            return sr.ReadToEnd();
        }
    }
}