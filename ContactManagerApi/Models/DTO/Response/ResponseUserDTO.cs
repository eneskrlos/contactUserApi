using ContactManagerApi.Utils;

namespace ContactManagerApi.Models.DTO.Response
{
    public class ResponseUserDTO
    {
        public ResponseApi ResponseApi { get; set; } = new ResponseApi();

        public List<UserDTO> UserDTOs { get; set; } = new List<UserDTO>();
    }
}
