using System.Threading.Tasks;
using CarePlus.Sandbox.Application.GoogleCloudStorage.Request;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

namespace CarePlus.Sandbox.Web.Services
{
    public class GoogleCloudStorageService : IGoogleCloudStorageService
    {
        private readonly HttpClient _httpClient;

        public GoogleCloudStorageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task UploadFileStream(UploadFileRequest request)
        {
            using var content = new MultipartFormDataContent();

            if (!string.IsNullOrEmpty(request.Bucket))
            {
                content.Add(new StringContent(request.Bucket), "Bucket");
            }

            if (!string.IsNullOrEmpty(request.ObjectName))
            {
                content.Add(new StringContent(request.ObjectName), "ObjectName");
            }

            if (!string.IsNullOrEmpty(request.ContentFile))
            {
                content.Add(new StringContent(request.ContentFile), "ContentFile");
            }

            if (request.FileStream != null)
            {
                if (request.FileStream.CanSeek)
                {
                    request.FileStream.Position = 0;
                }

                var streamContent = new StreamContent(request.FileStream);

                if (!string.IsNullOrEmpty(request.ContentFile))
                {
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(request.ContentFile);
                }

                // Use ObjectName as filename if available, otherwise default
                string fileName = !string.IsNullOrEmpty(request.ObjectName) ? request.ObjectName : "file";
                content.Add(streamContent, "FileStream", fileName);
            }

            var response = await _httpClient.PostAsync("upload", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
