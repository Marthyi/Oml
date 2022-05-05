using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Oml.Resources;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ResourcesPerformances
{
    [HtmlExporter]
    [MemoryDiagnoser]
    public class ResourceBenchmark
    {
        [Benchmark]
        public static string Read() => EmbeddedResources.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");

        [Benchmark]
        public static string Read1() => EmbeddedResourcesv1.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");

        //[Benchmark]
        //public string Read2() => EmbeddedResourcesv2.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");

        //[Benchmark]
        //public string Read3() => EmbeddedResourcesv3.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");

        [Benchmark]
        public static string Read4() => EmbeddedResourcesv4.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            string myVarName = null;
            Hello(myVarName);

            ArgumentNullException.ThrowIfNull(myVarName);

            //BenchmarkRunner.Run<ResourceBenchmark>();
        }

        public static void Hello([NotNull] string name, [CallerArgumentExpression("name")] string argName = "z")
        {
            int i = 2;
        }

    }
}