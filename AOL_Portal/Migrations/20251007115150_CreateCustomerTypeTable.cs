using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOL_Portal.Migrations
{
    /// <inheritdoc />
    public partial class CreateCustomerTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check if CustomerTypes table exists and drop it
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[CustomerTypes]', N'U') IS NOT NULL
                    DROP TABLE [dbo].[CustomerTypes];
            ");

            // Create the CustomerType table from scratch
            migrationBuilder.CreateTable(
                name: "CustomerType",
                columns: table => new
                {
                    CustomerTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                        .Annotation("Relational:ColumnOrder", 1),
                    CustomerTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                        .Annotation("Relational:ColumnOrder", 2)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerType", x => x.CustomerTypeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerType");
        }
    }
}
