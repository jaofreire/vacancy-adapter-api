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
    public class PaymentInfosController(IPaymentInfosService service) : ControllerBase
    {
        private readonly IPaymentInfosService _service = service;

        [HttpGet("user/{userId}")]
        [Authorize("EveryoneHasAccessPolicy")]
        public async Task<ActionResult<APIResponse<PaymentInfosModel>>> GetByUserId(Guid userId)
        {
            var result = await _service.GetByUserId(userId);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize("EveryoneHasAccessPolicy")]
        public async Task<ActionResult<APIResponse<PaymentInfosModel>>> Delete(Guid id)
        {
            var result = await _service.Remove(id);

            if (result.Code is 404)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize("EveryoneHasAccessPolicy")]
        public async Task<ActionResult<APIResponse<PaymentInfosModel>>> Update(Guid id, [FromBody] PaymentInfosModel model)
        {
            var result = await _service.Update(id, model);

            if (result.Code is 404)
                return NotFound(result);

            return Ok(result);
        }
    }
}

