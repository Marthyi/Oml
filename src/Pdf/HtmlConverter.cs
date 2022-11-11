using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace Pdf;

public class HtmlConverter : IDisposable
{
    private IPlaywright _playwright;

    private HtmlConverter(IPlaywright playwright)
    {
        _playwright = playwright;
    }

    public static async Task<HtmlConverter> CreateAsync()
    {
        var exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
        if (exitCode != 0)
        {
            throw new ApplicationException("cannot install browser to HtmlConverter");
        }

       var playwright = await Playwright.CreateAsync();

        return new HtmlConverter(playwright);
    }


    public async Task<byte[]> ConvertToPdf(string content, string header, string footer)
    {
        await using var browser = await _playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync();
        await page.SetContentAsync(content);

        var file = await page.PdfAsync(new PagePdfOptions()
        {
            PrintBackground = true,
            DisplayHeaderFooter = true,
            PreferCSSPageSize = true,
            HeaderTemplate = header,
            FooterTemplate = footer,
        });

        return file;
    }

    public void Dispose()
    {
        _playwright.Dispose();
    }
}
