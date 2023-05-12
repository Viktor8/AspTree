using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspTree.Model.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentNodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataNodes_DataNodes_ParentNodeId",
                        column: x => x.ParentNodeId,
                        principalTable: "DataNodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataNodes_ParentNodeId",
                table: "DataNodes",
                column: "ParentNodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataNodes");
        }
    }
}
