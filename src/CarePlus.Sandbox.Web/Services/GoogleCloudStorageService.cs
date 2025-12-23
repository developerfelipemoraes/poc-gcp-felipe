using System.Net.Http.Json;
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
            // Use PostAsJsonAsync. The IFormFileJsonConverter will handle serializing the File property to Base64.
            var response = await _httpClient.PostAsJsonAsync("upload", request);
            response.EnsureSuccessStatusCode();
        }
    }
}
