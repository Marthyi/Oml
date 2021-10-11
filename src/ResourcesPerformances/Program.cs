using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Oml.Resources;

namespace ResourcesPerformances
{
    [HtmlExporter]
    [MemoryDiagnoser]
    public class ResourceBenchmark
    {
        [Benchmark]
        public string Read() => EmbeddedResources.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");

        [Benchmark]
        public string Read1() => EmbeddedResourcesv1.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");

        //[Benchmark]
        //public string Read2() => EmbeddedResourcesv2.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");

        //[Benchmark]
        //public string Read3() => EmbeddedResourcesv3.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");

        [Benchmark]
        public string Read4() => EmbeddedResourcesv4.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<ResourceBenchmark>();
        }
    }
}