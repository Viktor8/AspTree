using AspTree.DTO;
using AspTree.Model;
using AspTree.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace AspTree.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreeController: ControllerBase
    {
        DataNodeService _nodeService;

        public TreeController(DataNodeService nodeService, ILogger<TreeController> logger)
        {
            _nodeService = nodeService;

        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _nodeService.GetByIdIncludingChildren(id);

            if (result is null)
            {
                return NotFound($"Journal record is created: {new Guid()}");
            }

            return Ok(result);
        }
    }
}
