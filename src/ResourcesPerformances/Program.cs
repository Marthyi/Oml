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
        public string Read()
        {
            return EmbeddedResources.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");
        }

        [Benchmark]
        public string Read2()
        {
            return EmbeddedResourcesv2.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");
        }

        [Benchmark]
        public string Read3()
        {
            return EmbeddedResourcesv3.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");
        }

        [Benchmark]
        public string Read4()
        {
            return EmbeddedResourcesv4.ReadAsText(typeof(ResourceBenchmark).Assembly, "resources.file.txt");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ResourceBenchmark>();
        }
    }
}