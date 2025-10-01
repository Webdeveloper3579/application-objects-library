using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOL_Portal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAolCustomerCustomField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerCustomFieldDescription",
                table: "AolCustomerCustomFields",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerCustomFieldName",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerCustomFieldStatus",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerCustomType",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerCustomFieldDescription",
                table: "AolCustomerCustomFields");

            migrationBuilder.DropColumn(
                name: "CustomerCustomFieldName",
                table: "AolCustomerCustomFields");

            migrationBuilder.DropColumn(
                name: "CustomerCustomFieldStatus",
                table: "AolCustomerCustomFields");

            migrationBuilder.DropColumn(
                name: "CustomerCustomType",
                table: "AolCustomerCustomFields");
        }
    }
}
