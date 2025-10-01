using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOL_Portal.Migrations
{
    /// <inheritdoc />
    public partial class AddAspNetCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CustomerType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetCustomers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetCustomers");
        }
    }
}
