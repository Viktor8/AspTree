using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspTree.Model.Migrations
{
    /// <inheritdoc />
    public partial class TableRenaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataNodes_DataNodes_ParentNodeId",
                table: "DataNodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataNodes",
                table: "DataNodes");

            migrationBuilder.RenameTable(
                name: "DataNodes",
                newName: "DataNode");

            migrationBuilder.RenameIndex(
                name: "IX_DataNodes_ParentNodeId",
                table: "DataNode",
                newName: "IX_DataNode_ParentNodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataNode",
                table: "DataNode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DataNode_DataNode_ParentNodeId",
                table: "DataNode",
                column: "ParentNodeId",
                principalTable: "DataNode",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataNode_DataNode_ParentNodeId",
                table: "DataNode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataNode",
                table: "DataNode");

            migrationBuilder.RenameTable(
                name: "DataNode",
                newName: "DataNodes");

            migrationBuilder.RenameIndex(
                name: "IX_DataNode_ParentNodeId",
                table: "DataNodes",
                newName: "IX_DataNodes_ParentNodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataNodes",
                table: "DataNodes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DataNodes_DataNodes_ParentNodeId",
                table: "DataNodes",
                column: "ParentNodeId",
                principalTable: "DataNodes",
                principalColumn: "Id");
        }
    }
}
