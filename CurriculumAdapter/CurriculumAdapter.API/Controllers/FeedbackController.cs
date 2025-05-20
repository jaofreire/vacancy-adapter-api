using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurriculumAdapter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController(IFeedbackService service) : ControllerBase
    {
        private readonly IFeedbackService _service = service;

        [HttpPost]
        public async Task<ActionResult<APIResponse<FeedbackModel>>> Register(FeedbackModel model)
        {
            var response = await _service.RegisterFeedback(model);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse<FeedbackModel>>> GetAll()
        {
            var response = await _service.GetAllFeedback();

            return Ok(response);
        }
    }
}
