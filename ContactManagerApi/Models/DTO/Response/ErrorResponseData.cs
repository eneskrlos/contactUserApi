using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContactManagerApi.Models.DTO.Response
{
    //Add new Error Response data
    public class ErrorResponseData
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
