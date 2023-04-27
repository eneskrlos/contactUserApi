using ContactManagerApi.Models;
using ContactManagerApi.Models.DTO.Request;
using ContactManagerApi.Models.DTO.Response;
using ContactManagerApi.Services.Interface;
using Microsoft.EntityFrameworkCore;
using ContactManagerApi.Utils;
using ContactManagerApi.Utils.Exceptions;

namespace ContactManagerApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserMangerContext _userMangerContext;

        private readonly ValidateUser _utilValidate;

        public UserService(UserMangerContext userMangerContext)
        {
            _userMangerContext = userMangerContext;
            _utilValidate = new ();
        }

        public User DTOtoDBO(RequestUserDTO userDTO)
        {
            return new User {
                Id = userDTO.Id,
                Firstname = userDTO.Firstname,
                Lastname = userDTO.Lastname,
                Username = userDTO.Username,
                Password = userDTO.Password
            };
        }

        public UserDTO DBOtoDTO(User? user)
        {
            if (user != null)
            {
                return new UserDTO
                {
                    Id = user.Id,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Username = user.Username
                };
            }
            return new UserDTO();
            
        }

        public async Task<ResponseUserDTO> CreateUser(RequestUserDTO request)
        {
            try
            {
                ResponseUserDTO responseUserDTO = new();
                User user = DTOtoDBO(request);
                _utilValidate.ValidateDataUser(user, _userMangerContext);

                user.Password = General.getSha1(request.Password);

                await _userMangerContext.AddAsync(user);
                await _userMangerContext.SaveChangesAsync();
                //le asignno un rol por defecto
                UserRole userRole = new() { IdUser = user.Id, IdRol = 1 };
                await _userMangerContext.AddAsync(userRole);
                await _userMangerContext.SaveChangesAsync();

                Contact contact = new()
                {
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Phone = request.Phone,
                    Email = request.Email,
                    DateOfBirth = request.DateOfBirth,
                    Owner = user.Id
                };

                await _userMangerContext.AddAsync(contact);
                await _userMangerContext.SaveChangesAsync();

                responseUserDTO.ResponseApi = new ResponseApi("ok", "Se a creado correctamente");
                responseUserDTO.UserDTOs.Add(DBOtoDTO(user));
                return responseUserDTO;
            }
            catch(UserIsAlreadyException e)
            {
                return ErrorResponseApi(e.Message);
            }
            catch (InvalidChracaterUserName e)
            {
                return ErrorResponseApi(e.Message);
            }
            catch (Exception e)
            {
                return ErrorResponseApi(e.Message);
            }
            
        }

        public async Task<ResponseUserDTO> DeleteUser(Guid id)
        {
            try
            {
                ResponseUserDTO responseUserDTO = new();
                var user = await _userMangerContext.Users.FindAsync(id);
                if (user != null)
                {
                    var contact = await _userMangerContext.Contacts.FirstAsync(x => x.Owner == id);
                    if (contact != null)
                    {
                        _userMangerContext.Remove(contact);
                    }

                    _userMangerContext.Remove(user);
                    await _userMangerContext.SaveChangesAsync();

                    responseUserDTO.ResponseApi = new ResponseApi("ok", "Se elimino el usuario correctamente.");
                    return new ResponseUserDTO();
                }
                else
                {
                    return ErrorResponseApi("Usuario no encontrado");
                }
            }
            catch (Exception e)
            {
                return ErrorResponseApi(e.Message);
            }
        }

        public async Task<ResponseUserDTO> GetAllUsers()
        {
            try
            {
                ResponseUserDTO responseUserDTO = new();
                var users = await _userMangerContext.Users.Select(u => new UserDTO
                {
                    Id = u.Id,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Username = u.Username
                }).ToListAsync();

                responseUserDTO.ResponseApi = new ResponseApi("ok", "Se listo los usuarios");
                responseUserDTO.UserDTOs = users;
                return responseUserDTO;
            }
            catch (Exception e)
            {
                return ErrorResponseApi(e.Message);
            }
        }

        public async Task<ResponseUserDTO> GetUserById(Guid id)
        {
            try
            {
                ResponseUserDTO responseUserDTO = new();
                var user = await _userMangerContext.Users.FindAsync(id);
                responseUserDTO.UserDTOs.Add(DBOtoDTO(user));
                return responseUserDTO;
            }
            catch (Exception e)
            {
                return ErrorResponseApi(e.Message);
            }
            
        }

        public async Task<ResponseUserDTO> UpdateUser(Guid id, RequestUserDTO userDto)
        {
            try
            {
                ResponseUserDTO responseuserDTO = new();
                User user = DTOtoDBO(userDto);

                _utilValidate.ValidateDataUser(user, _userMangerContext);

                var existeUser = await _userMangerContext.Users.FindAsync(id);
                if (existeUser == null)
                {
                    return ErrorResponseApi("Usuario no encontrado");
                }
                else
                {
                    existeUser.Id = user.Id;
                    existeUser.Username = user.Username;
                    existeUser.Firstname = user.Firstname;
                    existeUser.Lastname = user.Lastname;
                    existeUser.Password = user.Password;

                    _userMangerContext.Update(existeUser);
                    await _userMangerContext.SaveChangesAsync();

                    responseuserDTO.ResponseApi = new ResponseApi("ok", "Se actulizo correctamente.");
                    return responseuserDTO;
                }
            }
            catch (UserIsAlreadyException e)
            {
                return ErrorResponseApi(e.Message);
            }
            catch (InvalidChracaterUserName e)
            {
                return ErrorResponseApi(e.Message);
            }
            catch (Exception e)
            {
                return ErrorResponseApi(e.Message);
            }
        }

        private static ResponseUserDTO ErrorResponseApi(string message) => new() { ResponseApi = new ResponseApi("ko", message) };
    }
}
