using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsBlog.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnNewsImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbNews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsViewCount = table.Column<int>(type: "int", nullable: true),
                    NewsLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewsImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsContent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbNews", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbNews");
        }
    }
}
