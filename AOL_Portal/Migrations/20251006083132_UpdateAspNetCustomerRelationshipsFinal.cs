using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOL_Portal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAspNetCustomerRelationshipsFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a temporary table with the new structure
            migrationBuilder.CreateTable(
                name: "AspNetCustomers_New",
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
                    table.PrimaryKey("PK_AspNetCustomers_New", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetCustomers_New_AolCustomerCustomFields_CustomFieldId",
                        column: x => x.CustomFieldId,
                        principalTable: "AolCustomerCustomFields",
                        principalColumn: "CustomerFieldId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetCustomers_New_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Copy data from old table to new table (if any exists)
            migrationBuilder.Sql(@"
                INSERT INTO AspNetCustomers_New (CustomerId, CustomFieldId, CustomFieldValue, CreatedDate, ModifiedDate)
                SELECT 
                    CAST(CustomerId AS NVARCHAR(450)), 
                    CASE 
                        WHEN ISNUMERIC(CustomFieldId) = 1 THEN CAST(CustomFieldId AS INT)
                        ELSE 1  -- Default to first custom field if not numeric
                    END,
                    CustomFieldValue, 
                    CreatedDate, 
                    ModifiedDate
                FROM AspNetCustomers
            ");

            // Drop the old table
            migrationBuilder.DropTable(
                name: "AspNetCustomers");

            // Rename the new table to the original name
            migrationBuilder.RenameTable(
                name: "AspNetCustomers_New",
                newName: "AspNetCustomers");

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
            // Drop indexes
            migrationBuilder.DropIndex(
                name: "IX_AspNetCustomers_CustomFieldId",
                table: "AspNetCustomers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetCustomers_CustomerId",
                table: "AspNetCustomers");

            // Rename table back to temporary name
            migrationBuilder.RenameTable(
                name: "AspNetCustomers",
                newName: "AspNetCustomers_Old");

            // Create the old table structure
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

            // Copy data back (if any exists)
            migrationBuilder.Sql(@"
                INSERT INTO AspNetCustomers (CustomerId, CustomFieldId, CustomFieldValue, CreatedDate, ModifiedDate)
                SELECT 
                    CAST(CustomerId AS INT), 
                    CAST(CustomFieldId AS NVARCHAR(255)), 
                    CustomFieldValue, 
                    CreatedDate, 
                    ModifiedDate
                FROM AspNetCustomers_Old
            ");

            // Drop the temporary table
            migrationBuilder.DropTable(
                name: "AspNetCustomers_Old");
        }
    }
}
