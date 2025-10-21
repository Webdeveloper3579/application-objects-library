using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOL_Portal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAspNetCustomerStructureOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing table completely
            migrationBuilder.DropTable(
                name: "AspNetCustomers");

            // Create the new table with proper structure
            migrationBuilder.CreateTable(
                name: "AspNetCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CustomFieldId = table.Column<int>(type: "int", nullable: false),
                    CustomFieldValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetCustomers_AolCustomerCustomFields_CustomFieldId",
                        column: x => x.CustomFieldId,
                        principalTable: "AolCustomerCustomFields",
                        principalColumn: "CustomerFieldId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetCustomers_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_AspNetCustomers_CustomerId",
                table: "AspNetCustomers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetCustomers_CustomFieldId",
                table: "AspNetCustomers",
                column: "CustomFieldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the new table
            migrationBuilder.DropTable(
                name: "AspNetCustomers");

            // Recreate the old table structure
            migrationBuilder.CreateTable(
                name: "AspNetCustomers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomFieldId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CustomFieldValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetCustomers", x => x.CustomerId);
                });
        }
    }
}
