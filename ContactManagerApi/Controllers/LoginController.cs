using ContactManagerApi.Models.DTO.Request;
using ContactManagerApi.Models.DTO.Response;
using ContactManagerApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult<ResponseLoginDTO>> Login(RequestLoginDTO requestLoginDTO)
        {
            var response = await _loginService.Login(requestLoginDTO);
            if(response.ResponseApi.code == "ko") { 
                return BadRequest(response.ResponseApi);
            }

            return Ok(response);
        }
    }
}
