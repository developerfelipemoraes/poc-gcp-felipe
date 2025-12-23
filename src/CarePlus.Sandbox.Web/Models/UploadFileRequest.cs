using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using CarePlus.Sandbox.Web.Converters;

namespace CarePlus.Sandbox.Application.GoogleCloudStorage.Request
{
    public class UploadFileRequest
    {
        public string? Bucket { get; set; }
        public string? ObjectName { get; set; }

        [JsonConverter(typeof(IFormFileJsonConverter))]
        public IFormFile? File { get; set; }

        public string? ContentFile { get; set; }
    }
}
