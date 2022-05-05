using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Oml.Resources
{
    public static class EmbeddedResources
    {
        private readonly static ConcurrentDictionary<Assembly, string> _assemblyNames = new ConcurrentDictionary<Assembly, string>();
        private readonly static ConcurrentDictionary<string, HashSet<string>> _assemblyFilenames = new ConcurrentDictionary<string, HashSet<string>>();

        /// <summary>
        /// Open a read stream on embedded resource
        /// </summary>
        /// <param name="assembly">Assembly containing file</param>
        /// <param name="resourcePath">path to the resource</param>
        /// <returns></returns>
        public static Stream ReadAsStream(this Assembly assembly, string resourcePath)
        {
            assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));

            resourcePath = resourcePath switch
            {
                null => throw new ArgumentNullException(nameof(resourcePath)),
                string p when string.IsNullOrWhiteSpace(p) => throw new ArgumentException($"Resource with location '{p}' is invalid"),
                _ => resourcePath.Replace('/', '.').Replace('\\', '.').Replace(' ', '_')
            };

            resourcePath = resourcePath switch
            {
                string p when p.StartsWith('.') && p.Length == 1 => throw new ArgumentException($"Resource with location '{p}' is invalid"),
                string p when p.StartsWith('.') => p[1..],
                string p => resourcePath
            };

            string assemblyName = _assemblyNames.GetOrAdd(assembly, asm => asm.GetName().Name);
            HashSet<string> assemblyFilenames = _assemblyFilenames.GetOrAdd(assemblyName, _ => assembly
            .GetManifestResourceNames()
            .ToHashSet());

            string fullResourcePath = assemblyFilenames.SingleOrDefault(p => p.EndsWith(resourcePath));

            if (string.IsNullOrWhiteSpace(fullResourcePath))
            {
                throw new ArgumentException($"Resource with location '{resourcePath}' does not exists");
            }

            return assembly.GetManifestResourceStream(fullResourcePath);
        }

        /// <summary>
        /// Open a read stream on embedded resource
        /// </summary>
        /// <param name="assembly">Assembly containing file</param>
        /// <param name="resourcePath">path to the resource</param>
        /// <returns></returns>
        public static string ReadAsText(this Assembly assembly, string resourcePath)
        {
            using var sr = new StreamReader(assembly.ReadAsStream(resourcePath));
            return sr.ReadToEnd();
        }
    }
}