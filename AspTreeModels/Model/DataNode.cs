using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AspTree.Model
{
    [Table("DataNode")]
    public class DataNode
    {
        [Key]
        public int Id { get; init; }

        public string Name { get; set; } = new Guid().ToString();

        [JsonIgnore]
        public DataNode? ParentNode { get; set; }
        public int? ParentNodeId { get; set; }

        public List<DataNode>? Children { get; set; }

        public bool IsRoot { get => ParentNodeId is null; }
    }
}
