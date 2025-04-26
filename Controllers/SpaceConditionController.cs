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

        [HttpGet("{spaceId}/last7days")]
        public async Task<IActionResult> GetLast7Days(string spaceId)
        {
            var fromDate = DateTime.UtcNow.AddDays(-7);
            var conditions = await _context.SpaceConditions
                .Find(c => c.SpaceId == spaceId && c.Timestamp >= fromDate)
                .SortByDescending(c => c.Timestamp)
                .ToListAsync();
            return Ok(conditions);
        }

        [HttpGet("{spaceId}/last14days")]
        public async Task<IActionResult> GetLast14Days(string spaceId)
        {
            var fromDate = DateTime.UtcNow.AddDays(-14);
            var conditions = await _context.SpaceConditions
                .Find(c => c.SpaceId == spaceId && c.Timestamp >= fromDate)
                .SortByDescending(c => c.Timestamp)
                .ToListAsync();
            return Ok(conditions);
        }

        [HttpGet("{spaceId}/last30days")]
        public async Task<IActionResult> GetLast30Days(string spaceId)
        {
            var fromDate = DateTime.UtcNow.AddDays(-30);
            var conditions = await _context.SpaceConditions
                .Find(c => c.SpaceId == spaceId && c.Timestamp >= fromDate)
                .SortByDescending(c => c.Timestamp)
                .ToListAsync();
            return Ok(conditions);
        }

        [HttpGet("{spaceId}/last90days")]
        public async Task<IActionResult> GetLast90Days(string spaceId)
        {
            var fromDate = DateTime.UtcNow.AddDays(-90);
            var conditions = await _context.SpaceConditions
                .Find(c => c.SpaceId == spaceId && c.Timestamp >= fromDate)
                .SortByDescending(c => c.Timestamp)
                .ToListAsync();
            return Ok(conditions);
        }

        [HttpGet("{spaceId}/last180days")]
        public async Task<IActionResult> GetLast180Days(string spaceId)
        {
            var fromDate = DateTime.UtcNow.AddDays(-180);
            var conditions = await _context.SpaceConditions
                .Find(c => c.SpaceId == spaceId && c.Timestamp >= fromDate)
                .SortByDescending(c => c.Timestamp)
                .ToListAsync();
            return Ok(conditions);
        }
    }
}