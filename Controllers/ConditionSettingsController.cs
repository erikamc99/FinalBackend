using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Models;
using Muuki.Services;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/condition-settings")]
    public class ConditionSettingsController : ControllerBase
    {
        private readonly ConditionSettingsService _service;

        public ConditionSettingsController(ConditionSettingsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? type = null, [FromQuery] string? breed = null)
        {
            var result = await _service.GetAll(type, breed);
            return Ok(new { success = true, message = "List of condition settings", data = result });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConditionSettings settings)
        {
            var result = await _service.Create(settings);
            return Ok(new { success = true, message = "Condition setting created", data = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ConditionSettings settings)
        {
            await _service.Update(id, settings);
            return Ok(new { success = true, message = "Condition setting updated", data = (object)null });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.Delete(id);
            return Ok(new { success = true, message = "Condition setting deleted", data = (object)null });
        }
    }
}