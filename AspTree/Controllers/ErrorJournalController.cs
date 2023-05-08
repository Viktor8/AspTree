using AspTree.DTO;
using AspTree.Model.ErrorTracking;
using AspTree.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspTree.Controllers
{
    [Route("api/diagnostic/[controller]")]
    [ApiController]
    public class ErrorJournalController : ControllerBase
    {
        private ErrorJournalService _journal;

        public ErrorJournalController(ErrorJournalService journal)
        {
            _journal = journal;
        }


        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await _journal.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> GetRange(
            [FromBody] ErrorJournalGetRangeRequest? filters,
            [FromQuery] ErrorJournalGetRangePaginationParameters pagination)
        {
            var resultQuery = _journal.Find(filters?.fromUtc, filters?.toUtc, filters?.searchString);

            if (pagination.Skip > 0)
                resultQuery = resultQuery.Skip(pagination.Skip);

            if (pagination.Take > 0)
                resultQuery = resultQuery.Take(pagination.Take);

            var result = await resultQuery
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(result);
        }
    }
}
