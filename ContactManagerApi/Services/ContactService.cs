using ContactManagerApi.Models;
using ContactManagerApi.Models.DTO.request;
using ContactManagerApi.Models.DTO.Response;
using ContactManagerApi.Services.Interface;
using ContactManagerApi.Utils;
using ContactManagerApi.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace ContactManagerApi.Services
{

    public class ContactService  : IContactService
    {
        private readonly UserMangerContext _userMangerContext;

        private readonly ValidateContact _utils = new();

        
        public ContactService (UserMangerContext userMangerContext)
        {
            _userMangerContext = userMangerContext;
        }



        public Contact DTOtoDBO(RequestContactDTO contactDTO)
        {
            return new Contact()
            {
                Id = contactDTO.Id,
                Firstname = contactDTO.Firstname,
                Lastname = contactDTO.Lastname,
                DateOfBirth = contactDTO.DateOfBirth,
                Email = contactDTO.Email,
                Phone = contactDTO.Phone,
                Owner = contactDTO.Owner
            };
        }

        public ContactDTO DBOtoDTO(Contact contact)
        {   
            return new ContactDTO()
            {
                Id = contact.Id,
                Firstname = contact.Firstname,
                Lastname = contact.Lastname,
                DateOfBirth = contact.DateOfBirth,
                Email = contact.Email,
                Phone = contact.Phone,
                Owner = contact.Owner
            }; 
        }


        public async Task<ResponseContactDTO> GetAllContacts()
        {

            try
            {   

                ResponseContactDTO responseContactDTO = new();
                var contactDTOs = await _userMangerContext.Contacts.Select(a => new ContactDTO
                {
                    Id = a.Id,
                    Firstname = a.Firstname,
                    Lastname = a.Lastname,
                    DateOfBirth = a.DateOfBirth,
                    Phone = a.Phone,
                    Email = a.Email,
                    Owner = a.Owner
                }).ToListAsync();

                responseContactDTO.ResponseApi = new ResponseApi("ok", "Se listo los contactos.");
                responseContactDTO.ContactDtos = contactDTOs;
                return responseContactDTO;
            } 
            catch (Exception e)
            {
                return ErrorResponseApi(e.Message);
            }
            
        }

        public async Task<ResponseContactDTO> GetContactById(Guid id)
        {
            try
            {
                ResponseContactDTO responseContactDTO = new();
                var contact = await _userMangerContext.Contacts.FindAsync(id);

               if(contact != null)
                {
                    responseContactDTO.ResponseApi = new ResponseApi("ok", "Operacion exitosa");
                    var contactDto = DBOtoDTO(contact);
                    var list = new List<ContactDTO>
                    {
                        contactDto
                    };
                    responseContactDTO.ContactDtos = list;
                    return responseContactDTO;
                }
                else
                {
                    throw new ContactNotFoundException("No existe el elemento");
                }
            }
            catch (ContactNotFoundException e)
            {
                return ErrorResponseApi(e.Message);
            }
            catch (Exception e)
            {
                return ErrorResponseApi(e.Message);
            }
            
        }

       

       
        public async Task<ResponseContactDTO> CreateContact(RequestContactDTO contactDto)
        {
            try
            {
                ResponseContactDTO responseContactDTO = new();
                Contact contact = DTOtoDBO(contactDto);

                _utils.ValiateContact(contact, _userMangerContext);
               
                await _userMangerContext.AddAsync(contact);
                await _userMangerContext.SaveChangesAsync();

                var changeDto = DBOtoDTO(contact);
                var listcontact = new List<ContactDTO> {
                    changeDto
                };
                responseContactDTO.ContactDtos = listcontact;
                responseContactDTO.ResponseApi = new ResponseApi("ok", "Se creo correctamente");

                return responseContactDTO;
            }
            catch (EmailAdreesIsAlreadyException e)
            {
                return ErrorResponseApi(e.Message);
            }
            
            catch (ContactAgeException e)
            {
                return ErrorResponseApi(e.Message);
            }
            
            catch (ContactoNombreException e)
            {
                return ErrorResponseApi($"{e._contactName} inicia con digitos.");
            }
            catch (Exception e)
            {
                return ErrorResponseApi(e.Message);
            }
        }
        

        public async Task<ResponseContactDTO> UpdateContact(Guid id, RequestContactDTO contactDto)
        {
            try
            {
                ResponseContactDTO responseContactDTO = new();
                Contact contact = DTOtoDBO(contactDto);

                _utils.ValiateContact(contact, _userMangerContext);

                var existingContact = await _userMangerContext.Contacts.FindAsync(id);

                if (existingContact != null)
                {
                    existingContact.Firstname = contact.Firstname;
                    existingContact.Lastname = contact.Lastname;
                    existingContact.Email = contact.Email;
                    existingContact.Phone = contact.Phone;
                    existingContact.DateOfBirth = contact.DateOfBirth;
                    existingContact.Owner = contact.Owner;

                    _userMangerContext.Update(existingContact);
                    await _userMangerContext.SaveChangesAsync();


                    responseContactDTO.ResponseApi = new ResponseApi("ok", "Se actulizo correctamente");
                    return responseContactDTO;
                }
                else
                {
                    throw new ContactNotFoundException("No existe el elemento");
                }
            }
            catch (EmailAdreesIsAlreadyException e)
            {
                return ErrorResponseApi(e.Message);
            }

            catch (ContactAgeException e)
            {
                return ErrorResponseApi(e.Message);
            }

            catch (ContactoNombreException e)
            {
                return ErrorResponseApi($"{e._contactName} inicia con digitos.");
            }
            catch (ContactNotFoundException e)
            {
                return ErrorResponseApi(e.Message);
            }
            catch (Exception e)
            {
                return ErrorResponseApi(e.Message);
            }

        }
        

        public async Task<ResponseContactDTO> DeleteContact(Guid id)
        {
            try
            {
                ResponseContactDTO responseContactDTO = new();
                var contactDelete = await _userMangerContext.Contacts.FindAsync(id);

                if (contactDelete != null)
                {
                    _userMangerContext.Contacts.Remove(contactDelete);
                    await _userMangerContext.SaveChangesAsync();

                    responseContactDTO.ResponseApi = new ResponseApi("ok", "Se elemino correctamente");
                    return responseContactDTO;
                }
                else
                {
                    throw new ContactNotFoundException("No se pudo eliminar el contacto.Contacto no enconetrado");
                }
            }
            catch (ContactNotFoundException e)
            {
                return ErrorResponseApi(e.Message);
            }
            catch (Exception e) 
            { 
                return ErrorResponseApi(e.Message);
            }
            
        }

        private static ResponseContactDTO ErrorResponseApi(string mensaje) => new() { ResponseApi = new ResponseApi("ko", mensaje) };




    }
}
