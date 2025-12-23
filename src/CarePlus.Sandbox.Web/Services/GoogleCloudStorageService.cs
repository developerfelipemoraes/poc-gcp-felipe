using System.Net.Http.Headers;
using CarePlus.Sandbox.Application.GoogleCloudStorage.Request;

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
            if (request.File == null)
            {
                throw new ArgumentNullException(nameof(request.File));
            }

            using (var content = new MultipartFormDataContent())
            {
                // Add Bucket
                if (!string.IsNullOrEmpty(request.Bucket))
                {
                    content.Add(new StringContent(request.Bucket), "Bucket");
                }

                // Add ObjectName
                if (!string.IsNullOrEmpty(request.ObjectName))
                {
                    content.Add(new StringContent(request.ObjectName), "ObjectName");
                }

                // Add File
                var fileStream = request.File.OpenReadStream();
                var fileContent = new StreamContent(fileStream);

                if (!string.IsNullOrEmpty(request.File.ContentType))
                {
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(request.File.ContentType);
                }

                content.Add(fileContent, "File", request.File.FileName);

                // Post
                var response = await _httpClient.PostAsync("upload", content);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
