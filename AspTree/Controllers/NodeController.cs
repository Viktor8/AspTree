using AspTree.DTO;
using AspTree.Exceptions;
using AspTree.Model;
using AspTree.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace AspTree.Controllers
{
    [ApiController]
    [Tags("api/Tree.Node")]
    [Route("api/Tree.[controller]")]
    public class NodeController: ControllerBase
    {
        DataNodeService _nodeService;

        public NodeController(DataNodeService nodeService, ILogger<TreeController> logger)
        {
            _nodeService = nodeService;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DataNodeCreateRequest newNode)
        {
            var result = await _nodeService.Create(newNode.Name, newNode.ParentNodeId);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _nodeService.GetById(id);

            if (result is null)
                throw new SecureException("DataNode with such an id is not found");

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] DataNodeUpdateRequest nodeUpdate)
        {
            var updatedNode = await _nodeService.Update(id, nodeUpdate.Name, nodeUpdate.ParentNodeId);
            return Ok(updatedNode);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _nodeService.DeleteById(id);
            return NoContent();
        }
    }
}
