using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace CarePlus.Sandbox.Web.Converters
{
    public class IFormFileJsonConverter : JsonConverter<IFormFile>
    {
        public override IFormFile Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Deserialization is not required for the client-side request sending logic in this scope.
            // If needed, one would need to reconstruct a FormFile from Base64.
            throw new NotImplementedException("Deserializing IFormFile from JSON is not supported in this client.");
        }

        public override void Write(Utf8JsonWriter writer, IFormFile value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            using (var stream = value.OpenReadStream())
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                var bytes = memoryStream.ToArray();
                writer.WriteBase64StringValue(bytes);
            }
        }
    }
}
