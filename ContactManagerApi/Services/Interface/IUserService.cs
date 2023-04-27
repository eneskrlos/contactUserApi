using ContactManagerApi.Models.DTO.request;
using ContactManagerApi.Models.DTO.Request;
using ContactManagerApi.Models.DTO.Response;

namespace ContactManagerApi.Services.Interface
{
    public interface IUserService
    {
        Task<ResponseUserDTO> GetAllUsers();
        Task<ResponseUserDTO> GetUserById(Guid id);
        Task<ResponseUserDTO> CreateUser(RequestUserDTO userDto);
        Task<ResponseUserDTO> UpdateUser(Guid id, RequestUserDTO userDto);
        Task<ResponseUserDTO> DeleteUser(Guid id);
    }
}
