using ContactManagerApi.Models;
using ContactManagerApi.Models.DTO.request;
using ContactManagerApi.Models.DTO.Response;


namespace ContactManagerApi.Services.Interface
{
    public interface IContactService 
    {
        Task<ResponseContactDTO> GetAllContacts();
        Task<ResponseContactDTO> GetContactById(Guid id);
        Task<ResponseContactDTO> CreateContact(RequestContactDTO contact);
        Task<ResponseContactDTO> UpdateContact(Guid id, RequestContactDTO contact);
        Task<ResponseContactDTO> DeleteContact(Guid id);
    }
}
