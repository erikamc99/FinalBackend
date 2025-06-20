using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muuki.Models;
using Muuki.Data;
using MongoDB.Driver;

namespace Muuki.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/space-conditions")]
    public class SpaceConditionController : ControllerBase
    {
        private readonly MongoContext _context;

        public SpaceConditionController(MongoContext context)
        {
            _context = context;
        }

        [HttpGet("{spaceId}/history")]
        public async Task<IActionResult> GetHistory(string spaceId, [FromQuery] int days = 7)
        {
            if (days <= 0) days = 7;
            var fromDate = DateTime.UtcNow.AddDays(-days);
            var conditions = await _context.SpaceConditions
                .Find(c => c.SpaceId == spaceId && c.Timestamp >= fromDate)
                .SortByDescending(c => c.Timestamp)
                .ToListAsync();
            return Ok(conditions);
        }
    }
}