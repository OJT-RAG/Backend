using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using PdfiumViewer;

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
}
