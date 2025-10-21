using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOL_Portal.Migrations
{
    /// <inheritdoc />
    public partial class FixAspNetCustomerRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop tables that are not needed
            migrationBuilder.DropTable(
                name: "MessageAttachments");

            migrationBuilder.DropTable(
                name: "MessageTags");

            migrationBuilder.DropTable(
                name: "Messages");

            // Create a temporary table with the new structure
            migrationBuilder.CreateTable(
                name: "AspNetCustomers_Temp",
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
                    table.PrimaryKey("PK_AspNetCustomers_Temp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetCustomers_Temp_AolCustomerCustomFields_CustomFieldId",
                        column: x => x.CustomFieldId,
                        principalTable: "AolCustomerCustomFields",
                        principalColumn: "CustomerFieldId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetCustomers_Temp_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Copy data from old table to new table (if any exists)
            migrationBuilder.Sql(@"
                INSERT INTO AspNetCustomers_Temp (CustomerId, CustomFieldId, CustomFieldValue, CreatedDate, ModifiedDate)
                SELECT 
                    CAST(CustomerId AS NVARCHAR(450)), 
                    CAST(CustomFieldId AS INT), 
                    CustomFieldValue, 
                    CreatedDate, 
                    ModifiedDate
                FROM AspNetCustomers
            ");

            // Drop the old table
            migrationBuilder.DropTable(
                name: "AspNetCustomers");

            // Rename the temporary table to the original name
            migrationBuilder.RenameTable(
                name: "AspNetCustomers_Temp",
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
                newName: "AspNetCustomers_Temp");

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
                FROM AspNetCustomers_Temp
            ");

            // Drop the temporary table
            migrationBuilder.DropTable(
                name: "AspNetCustomers_Temp");

            // Recreate the message tables
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsImportant = table.Column<bool>(type: "bit", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    RecipientEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RecipientId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SenderEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                });

            migrationBuilder.CreateTable(
                name: "MessageAttachments",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageAttachments", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_MessageAttachments_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageTags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TagName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTags", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_MessageTags_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageAttachments_MessageId",
                table: "MessageAttachments",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageTags_MessageId",
                table: "MessageTags",
                column: "MessageId");
        }
    }
}
