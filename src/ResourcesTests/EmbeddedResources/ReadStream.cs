using FluentAssertions;
using Oml.Resources;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace ResourcesTests
{
    public class ReadStreamTests : EmbeddedResourcesTestsBase
    {
        [Fact]
        public void Success()
        {
            using Stream fileStream = EmbeddedResources.ReadAsStream(CurrentAssembly, "resources.file.txt");
            using var ms = new MemoryStream();

            fileStream.CopyTo(ms);
            byte[] data = ms.ToArray();

            data.Should().BeEquivalentTo(Encoding.UTF8.GetBytes(FileContent));
        }

        [Fact]
        public void NotExistingFileName()
        {
            string invalidPath = "resources.notexist.file.txt";
            Action action = () => EmbeddedResources.ReadAsStream(CurrentAssembly, invalidPath);

            action.Should()
                .Throw<ArgumentException>()
                .WithMessage($"Resource with location 'ResourcesTests.resources.notexist.file.txt' does not exists");
        }

        [Theory]
        [InlineData("")]
        [InlineData("                  ")]
        [InlineData("                  .")]
        [InlineData("                  .  ")]
        [InlineData(".")]
        public void InvalidFilePath(string invalidPath)
        {
            Action action = () => EmbeddedResources.ReadAsStream(CurrentAssembly, invalidPath);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AssemblyIsNull()
        {
            string invalidPath = "resources.notexist.file.txt";
            Action action = () => EmbeddedResources.ReadAsStream(null, invalidPath);

            action.Should()
                .Throw<ArgumentNullException>()
                .And
                .Message.Should().Contain("assembly");
        }

        [Fact]
        public void ResourcePathIsNull()
        {
            Action action = () => EmbeddedResources.ReadAsStream(CurrentAssembly, null);

            action.Should()
                .Throw<ArgumentNullException>()
                .And
                .Message.Should().Contain("resourcePath");
        }
    }
}