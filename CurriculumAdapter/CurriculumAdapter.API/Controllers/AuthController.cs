using CurriculumAdapter.API.DTOs;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurriculumAdapter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        private readonly IAuthService _service = service;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse<string>>> Login(LoginInputDTO input)
        {
            var result = await _service.Login(input);

            if (result.Code == 404)
                return NotFound(result);

            if(result.Code == 400)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
