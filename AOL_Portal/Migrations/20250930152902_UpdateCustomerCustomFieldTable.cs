using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOL_Portal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerCustomFieldTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "AolCustomerCustomFields",
                newName: "CustomerFieldId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "AolCustomerCustomFields",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustomType",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustomFieldType",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustomFieldStatus",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustomFieldName",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustomFieldLabel",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustomFieldDescription",
                table: "AolCustomerCustomFields",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500)
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "AolCustomerCustomFields",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2")
                .Annotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerFieldId",
                table: "AolCustomerCustomFields",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1)
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerFieldId",
                table: "AolCustomerCustomFields",
                newName: "CustomerId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "AolCustomerCustomFields",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustomType",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustomFieldType",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .OldAnnotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustomFieldStatus",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustomFieldName",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustomFieldLabel",
                table: "AolCustomerCustomFields",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerCustomFieldDescription",
                table: "AolCustomerCustomFields",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500)
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "AolCustomerCustomFields",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2")
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "AolCustomerCustomFields",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("Relational:ColumnOrder", 1)
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
