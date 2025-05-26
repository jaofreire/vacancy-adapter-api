using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurriculumAdapter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvisorController(IAdvisorService service) : ControllerBase
    {
        private readonly IAdvisorService _service = service;

        [HttpPost]
        public async Task<ActionResult<APIResponse<string>>> SendPromptToAdvisorAssistant([FromForm]string curriculumData)
        {
            var response = await _service.SendPromptToAdvisorAssistant(curriculumData);

            if(!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
