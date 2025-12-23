using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using CarePlus.Sandbox.Application.GoogleCloudStorage.Request;
using CarePlus.Sandbox.Web.Services;
using System.IO;

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
        public async Task<IActionResult> Upload(IFormFile file, string bucket, string objectName)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file.");
                return View("Index");
            }

            if (string.IsNullOrWhiteSpace(bucket))
            {
                ModelState.AddModelError("", "Bucket name is required.");
                return View("Index");
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var request = new UploadFileRequest
                    {
                        Bucket = bucket,
                        ObjectName = string.IsNullOrWhiteSpace(objectName) ? file.FileName : objectName,
                        FileStream = stream,
                        ContentFile = file.ContentType
                    };

                    await _storageService.UploadFileStream(request);

                    ViewBag.Message = $"File '{file.FileName}' uploaded successfully to bucket '{bucket}'.";
                }
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", $"Error uploading file: {ex.Message}");
            }

            return View("Index");
        }
    }
}
