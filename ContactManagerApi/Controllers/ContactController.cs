using ContactManagerApi.Models;
using ContactManagerApi.Models.DTO.request;
using ContactManagerApi.Models.DTO.Response;
using ContactManagerApi.Services;
using ContactManagerApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContactManagerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactService _service;

        public ContactController(ContactService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseContactDTO>> GetAll()
        {
            var response = await _service.GetAllContacts();

            if (response.ResponseApi.code.Equals("ko"))
            {
                return BadRequest(response.ResponseApi);
            }

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseContactDTO>> GetById(Guid id)
        {
            var response = await _service.GetContactById(id);

            if (response.ResponseApi.code.Equals("ko"))
            {
                return BadRequest(response.ResponseApi);
            }

            if (response.ContactDtos.Count == 0)
            {
                return NotFound(response.ResponseApi);
            }

            return response;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]RequestContactDTO contact)
        {   
            ResponseContactDTO response = await _service.CreateContact(contact);

            if (response.ResponseApi.code.Equals("ko"))
            {
                return BadRequest(response.ResponseApi);
            }

            return CreatedAtAction(nameof(GetById), new { id = response.ContactDtos.First().Id }, response.ContactDtos.First());
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RequestContactDTO contact)
        {
            if (id != contact.Id)
            {
                return BadRequest(new ResponseApi("ko", "No son correctos los identificadores"));
            }

            ResponseContactDTO response = await _service.UpdateContact(id, contact);

            if (response.ResponseApi.code.Equals("ok"))
            {
                return NoContent(); // Esto devolvera error 404 si no es satisfactorio el update si no, no devuelve nada.
            }
            else
            {
                return NotFound(response.ResponseApi);
            }
        }

        [Authorize(Policy = "AdministradorCubano")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            ResponseContactDTO response = await _service.DeleteContact(id);

            if (response.ResponseApi.code.Equals("ok"))
            {   
                return Ok(response); 
            }
            else
            {
                return NotFound(response);
            }
        }
    }
}
