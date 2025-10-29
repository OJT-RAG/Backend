using DocumentFormat.OpenXml.Packaging;
using System.Text;
using System.Text.Json;

namespace OJT_RAG.Services
{
    public static class DocTextExtractor
    {
        public static string ExtractTextToJson(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var wordDoc = WordprocessingDocument.Open(stream, false);

            var text = wordDoc.MainDocumentPart.Document.Body.InnerText;
            var json = JsonSerializer.Serialize(new { Content = text });
            return json;
        }
    }
}
