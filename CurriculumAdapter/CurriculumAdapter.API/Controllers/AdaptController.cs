using CurriculumAdapter.API.DTOs;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurriculumAdapter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdaptController(IAdaptService adaptService) : ControllerBase
    {
        private readonly IAdaptService _adaptService = adaptService;

        [HttpPost("sendPrompt")]
        public async Task<ActionResult<APIResponse<AssistantResponse>>> SendPromptToCurriculumAdapterAssistant([FromForm]SendPromptToCurriculumAdapterAssistantInputDTO inputDTO)
        {
            return await _adaptService.SendPromptToAssistant(inputDTO.RecaptchaToken, inputDTO.Description, inputDTO.UserSkills, inputDTO.File);
        }

    }
}
