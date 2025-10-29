using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using PdfiumViewer;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.Json;

public static class PdfConverter
{
    /// <summary>
    /// Chuyển trang đầu PDF thành ảnh JPEG
    /// </summary>
    public static byte[] ConvertPdfPageToImage(string pdfPath, int pageNumber = 0, int dpi = 150)
    {
        using var doc = PdfDocument.Load(pdfPath);
        using var image = doc.Render(pageNumber, dpi, dpi, true);
        using var ms = new MemoryStream();
        image.Save(ms, ImageFormat.Jpeg);
        return ms.ToArray();
    }

    /// <summary>
    /// Chuyển file .docx thành JSON (chỉ trích xuất nội dung văn bản)
    /// </summary>
    public static string ConvertDocToJson(IFormFile file)
    {
        using var stream = new MemoryStream();
        file.CopyTo(stream);
        stream.Position = 0;

        using var wordDoc = WordprocessingDocument.Open(stream, false);
        var body = wordDoc.MainDocumentPart?.Document?.Body;

        if (body == null)
            return "{}";

        var paragraphs = body.Elements<Paragraph>();
        var textList = new List<string>();

        foreach (var para in paragraphs)
        {
            var text = para.InnerText?.Trim();
            if (!string.IsNullOrEmpty(text))
                textList.Add(text);
        }

        var docJson = new
        {
            FileName = file.FileName,
            Paragraphs = textList
        };

        return JsonSerializer.Serialize(docJson, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
