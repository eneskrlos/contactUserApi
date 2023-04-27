using ContactManagerApi.Models;
using ContactManagerApi.Models.DTO.Request;
using ContactManagerApi.Models.DTO.Response;
using ContactManagerApi.Services;
using ContactManagerApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagerApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
                _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ResponseUserDTO> GetAll()
        {
            return await _userService.GetAllUsers();
        }

        [Authorize]
        [HttpGet("id")]
        public async Task<ActionResult<ResponseUserDTO>> GetById(Guid id)
        {
            var response = await _userService.GetUserById(id);

            if(response.UserDTOs.Count == 0)
            {
                return NotFound(response.ResponseApi);
            }

            return response;
        }

        
        [HttpPost]
        public async Task<ActionResult<ResponseUserDTO>> Create([FromBody] RequestUserDTO requestUserDTO)
        {
            var response = await _userService.CreateUser(requestUserDTO);
            if (response.ResponseApi.code.Equals("ko"))
            {
                return BadRequest(response.ResponseApi);
            }
            return CreatedAtAction(nameof(GetById), new { id = response.UserDTOs.First().Id }, response.UserDTOs.First()); 
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ResponseUserDTO>> Update(Guid id, [FromBody] RequestUserDTO requestUserDTO)
        {

            if (id != requestUserDTO.Id)
            {
                return BadRequest(new ResponseApi("ko", "No son correctos los identificadores"));
            }

            var response = await _userService.UpdateUser(id, requestUserDTO);

            if (response.ResponseApi.code.Equals("ok"))
            {
                return Ok(response.ResponseApi);
            }
            else
            {
                return NotFound(response.ResponseApi);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseUserDTO>> Delete(Guid id)
        {
            var response  = await _userService.DeleteUser(id);
            if (response.ResponseApi.code.Equals("ok"))
            {
                return Ok(response.ResponseApi);
            }
            else
            {
                return NotFound(response);
            }
        }

    }
}
