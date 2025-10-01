using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOL_Portal.Migrations
{
    /// <inheritdoc />
    public partial class AddAolCustomerCustomFieldTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AolCustomerCustomFields",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCustomFieldLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerCustomFieldType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AolCustomerCustomFields", x => x.CustomerId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AolCustomerCustomFields");
        }
    }
}
