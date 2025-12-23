using Microsoft.AspNetCore.Http;

namespace CarePlus.Sandbox.Application.GoogleCloudStorage.Request
{
    public class UploadFileRequest
    {
        public string? Bucket { get; set; }
        public string? ObjectName { get; set; }
        public IFormFile? File { get; set; }
        public string? ContentFile { get; set; }
    }
}
