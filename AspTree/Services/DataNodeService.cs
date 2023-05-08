using AspTree.DTO;
using AspTree.Exceptions;
using AspTree.Model;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace AspTree.Services
{
    public class DataNodeService
    {
        DataTreeContext _dbContext;

        public DataNodeService(DataTreeContext context)
        {
            _dbContext = context;
        }

        public async Task<DataNode> GetById(int id)
        {
            return await _dbContext.DataNodeRepository
                .Where(node => node.Id == id)
                .SingleOrDefaultAsync() 
                ?? ThrowNodeNotFound();
        }

        public async Task<DataNode?> GetByIdIncludingChildren(int id)
        {
            var result = await _dbContext.DataNodeRepository
                .Where(node => node.Id == id)
                .Include(n => n.Children)
                .SingleOrDefaultAsync()
                ?? ThrowNodeNotFound();


            foreach (var node in result.Children ?? throw new Exception("Impossible!!!"))
                await GetByIdIncludingChildren(node.Id);

            return result;
        }



        public async Task<DataNode> Create(DataNodeCreateRequest nodeCreateRequest)
        {
            var node = new DataNode
            {
                Name = nodeCreateRequest.Name,
                ParentNodeId = nodeCreateRequest.ParentNodeId
            };

            await VerifyParrentNodeExistance(node.ParentNodeId);
            await VerifyNameUniquenessAmongSiblings(node.ParentNodeId, node.Name);

            _dbContext.DataNodeRepository.Add(node);
            await _dbContext.SaveChangesAsync();
            return node;
        }

        public async Task DeleteById(int nodeId)
        {
            if (await _dbContext.DataNodeRepository.Where(n => n.ParentNodeId == nodeId).AnyAsync())
                throw new SecureException("You have to delete all children nodes first");

            var numRowsDeleated = await _dbContext.DataNodeRepository.Where(n => n.Id == nodeId).ExecuteDeleteAsync();
            if (numRowsDeleated == 0)
                ThrowNodeNotFound();
        }


        public async Task<DataNode> Update(int id, DataNodeUpdateRequest nodeUpdate)
        {
            var node = await GetById(id);

            await VerifyParrentNodeExistance(nodeUpdate.ParentNodeId);
            await VerifyAbsenceOfCyclicReferences(node.Id, nodeUpdate.ParentNodeId);
            await VerifyNameUniquenessAmongSiblings(nodeUpdate.ParentNodeId, nodeUpdate.Name);


            node.Name = nodeUpdate.Name;
            node.ParentNodeId = nodeUpdate.ParentNodeId;
            await _dbContext.SaveChangesAsync();

            return node;
        }

        private async Task VerifyParrentNodeExistance(int? parrentId)
        {
            if (parrentId is null)
                return;

            var parentNode = await _dbContext.DataNodeRepository.Where(n => n.Id == parrentId).SingleOrDefaultAsync();
            if (parentNode is null)
                throw new SecureException($"Node must have a valid {nameof(DataNode.ParentNodeId)}, or be a root node with {nameof(DataNode.ParentNodeId)} of null.");
        }

        private async Task VerifyAbsenceOfCyclicReferences(int nodeId, int? newParrentId)
        {
            if (newParrentId is null)
                return;

            var newParentsChain = await GetNodeParentChainAsync(newParrentId.Value);
            if (newParentsChain.Any(p => p == nodeId))
                throw new SecureException("This change would lead to the creation of cyclic reference. You can not assign this node to this parent.");
        }

        private static DataNode ThrowNodeNotFound()
        {
            throw new SecureException("DataNode with such an id is not found");
        }


        public async Task<List<int>> GetNodeParentChainAsync(int nodeId)
        {
            List<int> result = new List<int>() { nodeId };

            while (true)
            {
                var parentId = await _dbContext.DataNodeRepository.Where(n => n.Id == result.Last()).Select(n => n.ParentNode).SingleOrDefaultAsync();
                if (parentId is null)
                    break;
                result.Add(parentId.Id);
            }

            return result;
        }

        private async Task VerifyNameUniquenessAmongSiblings(int? parrentId, string name)
        {
            var hasSiblingsWithSameName = await _dbContext.DataNodeRepository.Where(n => n.ParentNodeId == parrentId && n.Name == name).AnyAsync();

            if (hasSiblingsWithSameName)
                throw new SecureException("Node name must be unique among its immediate siblings.");
        }
    }
}
