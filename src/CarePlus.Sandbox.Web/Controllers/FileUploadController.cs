using Microsoft.AspNetCore.Mvc;
using CarePlus.Sandbox.Application.GoogleCloudStorage.Request;
using CarePlus.Sandbox.Web.Services;

namespace CarePlus.Sandbox.Web.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IGoogleCloudStorageService _storageService;

        public FileUploadController(IGoogleCloudStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadFileRequest request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file.");
                return View("Index");
            }

            if (string.IsNullOrEmpty(request.Bucket))
            {
                ModelState.AddModelError("Bucket", "Bucket name is required.");
                return View("Index");
            }

            // If ObjectName is empty, use the file name
            if (string.IsNullOrEmpty(request.ObjectName))
            {
                request.ObjectName = request.File.FileName;
            }

            try
            {
                await _storageService.UploadFileStream(request);

                ViewBag.Message = $"File '{request.ObjectName}' uploaded successfully to bucket '{request.Bucket}'.";
                return View("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error uploading file: {ex.Message}");
                return View("Index");
            }
        }
    }
}
