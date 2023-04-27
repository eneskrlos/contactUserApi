using ContactManagerApi.Models;
using ContactManagerApi.Models.DTO.Request;
using ContactManagerApi.Models.DTO.Response;
using ContactManagerApi.Services.Interface;
using ContactManagerApi.Utils;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace ContactManagerApi.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserMangerContext _userMangerContext;
        private IConfiguration _config;

        public LoginService(UserMangerContext userMangerContext, IConfiguration config)
        {
            _userMangerContext = userMangerContext;
            _config = config;
        }



        public async Task<ResponseLoginDTO> Login(RequestLoginDTO requestLoginDTO)
        {
            var user = await _userMangerContext.Users.SingleOrDefaultAsync(u => u.Username == requestLoginDTO.Username 
            && u.Password == General.getSha1(requestLoginDTO.Password));
            
            if (user == null)
            {
                return new ResponseLoginDTO { 
                    ResponseApi = new ResponseApi("ko","No son correctas las credenciales"),
                    Token = string.Empty
                };
            }

            string jwtToken = await GenerateToken(user);

            return new ResponseLoginDTO {
                ResponseApi = new ResponseApi("ok", "Se autentico correctamente."),
                Token = jwtToken
            };
        }

        private async Task<string> GenerateToken(User user)
        {
            string [] roles = await ObtenerRoles(user);
            string phone = await ObtenerTelefonoContacto(user);
            //string serializar = RolSingleton.Instance.SerializarRol(roles.ToArray());
            //string rol = RolSingleton.Instance.ObtenerRol(serializar, "Administrator");
            var claims = new[]
            {
                new Claim("GivenName",user.Firstname),
                new Claim("Surname",user.Lastname),
                new Claim("Phone",phone),
                new Claim("Roles", JsonSerializer.Serialize(roles)),
                new Claim(ClaimTypes.Country, EsCubano(phone))
            };
            
            string secret = _config.GetSection("JWT:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                    issuer: "GSI Challenge Authenticator",
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: creds,
                    claims: claims
                );

            String token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            
            return token;
        }

        public string EsCubano(string phone)
        {
            string phonePlus = "+" + phone;
            if (phonePlus.Contains("+53"))
            {
                return "CU";
            }
            else
            {
                return "";
            }
             
        }

        public async Task<string[]> ObtenerRoles(User user)
        {
            var result = from role in _userMangerContext.Roles
                         join user_role in _userMangerContext.UserRoles on role.Id equals user_role.IdRol
                         where user_role.IdUser == user.Id
                         select role.Rol ;            
            string[] roles = await result.ToArrayAsync();
            return roles;
        }

        public async Task<string> ObtenerTelefonoContacto(User user)
        {
            var contact = await _userMangerContext.Contacts.FirstAsync(x => x.OwnerNavigation.Id == user.Id && x.Firstname.Equals(user.Firstname) && x.Lastname.Equals(user.Lastname));
            return contact.Phone;
        }
    }
}
