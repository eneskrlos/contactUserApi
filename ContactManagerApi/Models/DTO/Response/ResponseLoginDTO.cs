using ContactManagerApi.Utils;

namespace ContactManagerApi.Models.DTO.Response
{
    public class ResponseLoginDTO
    {
        public ResponseApi ResponseApi { get; set; } = new ResponseApi();
        public string Token { get; set; } = null!;
        

    }
}
