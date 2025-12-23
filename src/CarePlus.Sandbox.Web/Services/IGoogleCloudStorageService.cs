using System.Threading.Tasks;
using CarePlus.Sandbox.Application.GoogleCloudStorage.Request;

namespace CarePlus.Sandbox.Web.Services
{
    public interface IGoogleCloudStorageService
    {
        Task UploadFileStream(UploadFileRequest request);
    }
}
