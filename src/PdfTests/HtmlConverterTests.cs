using Pdf;

namespace PdfTests;

public class HtmlConverterTests
{
    [Fact]
    public async Task CreateSimpleFile()
    {
        using var converter = await HtmlConverter.CreateAsync();
        var file = await converter.ConvertToPdf(GetContent("demo.html"), GetContent("header.html"), GetContent("footer.html"));
        File.WriteAllBytes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf"), file);
    }

    private string GetContent(string filename)
    {
        string path = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "html", filename);
        return File.ReadAllText(path);

    }
}