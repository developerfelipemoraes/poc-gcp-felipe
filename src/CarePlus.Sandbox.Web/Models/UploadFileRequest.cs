using System.IO;

namespace CarePlus.Sandbox.Application.GoogleCloudStorage.Request
{
    public class UploadFileRequest
    {
        public string? Bucket { get; set; }
        public string? ObjectName { get; set; }
        public Stream? FileStream { get; set; }
        public string? ContentFile { get; set; }
    }
}
