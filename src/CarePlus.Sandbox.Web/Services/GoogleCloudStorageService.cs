using System.Threading.Tasks;
using CarePlus.Sandbox.Application.GoogleCloudStorage.Request;
using Google.Cloud.Storage.V1; // Assuming Google.Cloud.Storage.V1 package is referenced
using System.IO;

namespace CarePlus.Sandbox.Web.Services
{
    public class GoogleCloudStorageService : IGoogleCloudStorageService
    {
        public async Task UploadFileStream(UploadFileRequest request)
        {
            // Note: This requires the 'Google.Cloud.Storage.V1' NuGet package.
            // Ensure GOOGLE_APPLICATION_CREDENTIALS environment variable is set or authentication is handled.

            // If the client instantiation needs specific configuration, it should be done here or injected.
            // For simplicity, we use the default create which uses the environment credentials.
            var storageClient = await StorageClient.CreateAsync();

            // Ensure the stream is at the beginning
            if (request.FileStream.CanSeek)
            {
                request.FileStream.Position = 0;
            }

            await storageClient.UploadObjectAsync(
                request.Bucket,
                request.ObjectName,
                request.ContentFile, // Using ContentFile as ContentType
                request.FileStream
            );
        }
    }
}
