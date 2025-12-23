using System.Threading.Tasks;
using CarePlus.Sandbox.Application.GoogleCloudStorage.Request;
using System.Net.Http;
using System.Net.Http.Json;

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
            var response = await _httpClient.PostAsJsonAsync("upload", request);
            response.EnsureSuccessStatusCode();
        }
    }
}
