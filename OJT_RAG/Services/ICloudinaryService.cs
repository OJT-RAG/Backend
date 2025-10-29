using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OJT_RAG.Services
{
    public interface ICloudinaryService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<string> UploadPdfAsImageAsync(IFormFile file);
    }
}
