using ContactManagerApi.Models.DTO.Request;
using ContactManagerApi.Models.DTO.Response;

namespace ContactManagerApi.Services.Interface
{
    public interface ILoginService
    {
        Task<ResponseLoginDTO> Login(RequestLoginDTO requestLoginDTO);   
    }
}
