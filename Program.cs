using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use.
        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Console Stream Uploader ===");

            // Set a default timeout for large file uploads
            client.Timeout = TimeSpan.FromMinutes(30);

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Upload File (Stream)");
                Console.WriteLine("2. Exit");
                Console.Write("Select an option: ");

                var option = Console.ReadLine();

                if (option == "1")
                {
                    await HandleUploadAsync();
                }
                else if (option == "2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
        }

        static async Task HandleUploadAsync()
        {
            try
            {
                // 1. Get Parameters
                Console.Write("Enter Server URL (e.g., http://localhost:5000/UploadFileStream): ");
                string url = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(url))
                {
                    Console.WriteLine("URL is required.");
                    return;
                }

                Console.Write("Enter Local File Path: ");
                string filePath = Console.ReadLine()?.Trim() ?? "";

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"File not found: {filePath}");
                    return;
                }

                Console.Write("Enter Bucket Name: ");
                string bucket = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Enter Object Name (Target filename): ");
                string objectName = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Enter Content Type (e.g., application/pdf): ");
                string contentType = Console.ReadLine()?.Trim() ?? "application/octet-stream";

                // 2. Prepare the Stream Request
                // We use MultipartFormDataContent because it is the standard way to stream files
                // alongside other parameters (Bucket, ObjectName) in a single request.
                //
                // NOTE: The Server Controller must use [FromForm] to accept this request.
                // If the server uses [FromBody], it expects JSON, which cannot easily stream files.

                using (var content = new MultipartFormDataContent())
                {
                    // Add parameters
                    if (!string.IsNullOrEmpty(bucket))
                    {
                        content.Add(new StringContent(bucket), "Bucket");
                    }

                    if (!string.IsNullOrEmpty(objectName))
                    {
                        content.Add(new StringContent(objectName), "ObjectName");
                    }

                    // We can send ContentFile as a parameter if the server expects it as a string field
                    content.Add(new StringContent(contentType), "ContentFile");

                    // Add File Stream
                    // We open the file stream here. It will be closed when the content is disposed (after sending).
                    // We use FileShare.Read to ensure we can read it even if locked for reading elsewhere.
                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var streamContent = new StreamContent(fileStream);

                        // Set the content type for the file part
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                        // "FileStream" is the name of the form field expected by the server model binder
                        // matching the property `public Stream? FileStream { get; set; }`
                        content.Add(streamContent, "FileStream", Path.GetFileName(filePath));

                        Console.WriteLine("Uploading...");

                        // 3. Send Request
                        // PostAsync will stream the content. It does NOT load the whole file into memory.
                        var response = await client.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("Success!");
                            Console.WriteLine($"Server Response: {responseBody}");
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode}");
                            var errorBody = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Details: {errorBody}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
