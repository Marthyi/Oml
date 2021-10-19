using FluentAssertions;
using Oml.Resources;
using Xunit;

namespace ResourcesTests
{
    public class ReadTextTests : EmbeddedResourcesTestsBase
    {
        [Fact]
        public void ReadText()
        {
            EmbeddedResources.ReadAsText(CurrentAssembly, "resources.file.txt")
                .Should()
                .Be("hello world");
        }

        [Fact]
        public void ReadTextWithPathAsFilePath()
        {
            EmbeddedResources.ReadAsText(CurrentAssembly, "resources/file.txt")
                .Should()
                .Be("hello world");
        }

        [Fact]
        public void ReadTextWithPathAsRootFilePath()
        {
            EmbeddedResources.ReadAsText(CurrentAssembly, "/resources/file.txt")
                .Should()
                .Be("hello world");
        }

        [Fact]
        public void ReadTextWithPathContainingSpace()
        {
            EmbeddedResources.ReadAsText(CurrentAssembly, "/resources/Sub Folder_/subfile.txt")
                .Should()
                .Be("hello world");
        }
    }
}