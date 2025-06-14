using CurriculumAdapter.API.DTOs;
using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurriculumAdapter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService service) : ControllerBase
    {
        private readonly IUserService _service = service;

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse<UserModel>>> Register(RegisterUserInputDTO input)
        {
            var result = await _service.Register(input);

            if(result.Code == 400)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        [Authorize("Admin")]
        public async Task<ActionResult<APIResponse<UserModel>>> GetAll()
        {
            var result = await _service.GetAll();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize("EveryoneHasAccessPolicy")]
        public async Task<ActionResult<APIResponse<UserModel>>> GetById(Guid id)
        {
            var result = await _service.GetById(id);

            if(result.Code == 404)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize("EveryoneHasAccessPolicy")]
        public async Task<ActionResult<APIResponse<UserModel>>> Delete(Guid id)
        {
            var result = await _service.Delete(id);

            if (result.Code == 404)
                return NotFound(result);

            return Ok(result);
        }

    }
}
