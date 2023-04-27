using ContactManagerApi.Utils;

namespace ContactManagerApi.Models.DTO.Response
{
    public class ResponseContactDTO
    {
        public ResponseApi ResponseApi { get; set; } = new ResponseApi();
        public List<ContactDTO> ContactDtos { get; set; } =  null!;

        
    }
}
