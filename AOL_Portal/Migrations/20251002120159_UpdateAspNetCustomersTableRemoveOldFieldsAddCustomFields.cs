using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOL_Portal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAspNetCustomersTableRemoveOldFieldsAddCustomFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerAddress",
                table: "AspNetCustomers");

            migrationBuilder.RenameColumn(
                name: "CustomerType",
                table: "AspNetCustomers",
                newName: "CustomFieldValue");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "AspNetCustomers",
                newName: "CustomFieldId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetCustomers",
                newName: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomFieldValue",
                table: "AspNetCustomers",
                newName: "CustomerType");

            migrationBuilder.RenameColumn(
                name: "CustomFieldId",
                table: "AspNetCustomers",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "AspNetCustomers",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "CustomerAddress",
                table: "AspNetCustomers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
