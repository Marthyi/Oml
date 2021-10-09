using FluentAssertions;
using Oml.Resources;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;

namespace ResourcesTests
{
    public class EmbeddedResourcesTests
    {
        private static readonly Assembly CurrentAssembly = typeof(EmbeddedResourcesTests).Assembly;
        private const string FileContent = "hello world";

        [Fact]
        public void ReadStream()
        {
            using Stream fileStream = EmbeddedResources.ReadAsStream(CurrentAssembly, "resources.file.txt");
            using var ms = new MemoryStream();

            fileStream.CopyTo(ms);
            byte[] data = ms.ToArray();

            data.Should().BeEquivalentTo(Encoding.UTF8.GetBytes(FileContent));
        }

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
        public void ReadStreamOfNotExistingFile()
        {
            string invalidPath = "resources.notexist.file.txt";
            Action action = () => EmbeddedResources.ReadAsStream(CurrentAssembly, invalidPath);

            action.Should()
                .Throw<ArgumentException>()
                .WithMessage($"Resource with location 'ResourcesTests.resources.notexist.file.txt' does not exists");
        }
    }
}