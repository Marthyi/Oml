using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Oml.Resources
{
    public static class EmbeddedResourcesv4
    {
        private static ConcurrentDictionary<Assembly, string> _assemblyNames = new ConcurrentDictionary<Assembly, string>();
        private static ConcurrentDictionary<string, string[]> _assemblyFilenames = new ConcurrentDictionary<string, string[]>();

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

            string assemblyName = _assemblyNames.GetOrAdd(assembly, asm => asm.GetName().Name);
            string[] assemblyFilenames = _assemblyFilenames.GetOrAdd(assemblyName, _ => assembly.GetManifestResourceNames());

            resourcePath = $"{assemblyName}.{resourcePath}";

            if (!assemblyFilenames.Contains(resourcePath))
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