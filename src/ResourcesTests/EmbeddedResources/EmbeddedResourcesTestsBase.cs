using System.Reflection;

namespace ResourcesTests
{
    public class EmbeddedResourcesTestsBase
    {
        protected static readonly Assembly CurrentAssembly = typeof(EmbeddedResourcesTestsBase).Assembly;
        protected const string FileContent = "hello world";
    }
}