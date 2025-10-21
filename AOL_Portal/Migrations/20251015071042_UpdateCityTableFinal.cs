using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOL_Portal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCityTableFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomObjectFieldValues");

            migrationBuilder.DropTable(
                name: "CustomObjectRelationshipInstances");

            migrationBuilder.DropTable(
                name: "CustomObjectFields");

            migrationBuilder.DropTable(
                name: "CustomObjectInstances");

            migrationBuilder.DropTable(
                name: "CustomObjectRelationships");

            migrationBuilder.DropTable(
                name: "CustomObjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "City");

            migrationBuilder.AlterColumn<string>(
                name: "CityCode",
                table: "City",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_City",
                table: "City",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_City",
                table: "City");

            migrationBuilder.RenameTable(
                name: "City",
                newName: "Cities");

            migrationBuilder.AlterColumn<string>(
                name: "CityCode",
                table: "Cities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "CityId");

            migrationBuilder.CreateTable(
                name: "CustomObjects",
                columns: table => new
                {
                    CustomObjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IconName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSystemObject = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ObjectApiName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ObjectDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ObjectName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomObjects", x => x.CustomObjectId);
                });

            migrationBuilder.CreateTable(
                name: "CustomObjectFields",
                columns: table => new
                {
                    FieldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomObjectId = table.Column<int>(type: "int", nullable: false),
                    LookupObjectId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DefaultValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    FieldApiName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FieldLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsUnique = table.Column<bool>(type: "bit", nullable: false),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PicklistValues = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ValidationRules = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomObjectFields", x => x.FieldId);
                    table.ForeignKey(
                        name: "FK_CustomObjectFields_CustomObjects_CustomObjectId",
                        column: x => x.CustomObjectId,
                        principalTable: "CustomObjects",
                        principalColumn: "CustomObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomObjectFields_CustomObjects_LookupObjectId",
                        column: x => x.LookupObjectId,
                        principalTable: "CustomObjects",
                        principalColumn: "CustomObjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomObjectInstances",
                columns: table => new
                {
                    InstanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomObjectId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstanceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomObjectInstances", x => x.InstanceId);
                    table.ForeignKey(
                        name: "FK_CustomObjectInstances_CustomObjects_CustomObjectId",
                        column: x => x.CustomObjectId,
                        principalTable: "CustomObjects",
                        principalColumn: "CustomObjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomObjectRelationships",
                columns: table => new
                {
                    RelationshipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceObjectId = table.Column<int>(type: "int", nullable: false),
                    TargetObjectId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RelationshipDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RelationshipName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RelationshipType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SourceLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomObjectRelationships", x => x.RelationshipId);
                    table.ForeignKey(
                        name: "FK_CustomObjectRelationships_CustomObjects_SourceObjectId",
                        column: x => x.SourceObjectId,
                        principalTable: "CustomObjects",
                        principalColumn: "CustomObjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomObjectRelationships_CustomObjects_TargetObjectId",
                        column: x => x.TargetObjectId,
                        principalTable: "CustomObjects",
                        principalColumn: "CustomObjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomObjectFieldValues",
                columns: table => new
                {
                    ValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldId = table.Column<int>(type: "int", nullable: false),
                    InstanceId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FieldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomObjectFieldValues", x => x.ValueId);
                    table.ForeignKey(
                        name: "FK_CustomObjectFieldValues_CustomObjectFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "CustomObjectFields",
                        principalColumn: "FieldId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomObjectFieldValues_CustomObjectInstances_InstanceId",
                        column: x => x.InstanceId,
                        principalTable: "CustomObjectInstances",
                        principalColumn: "InstanceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomObjectRelationshipInstances",
                columns: table => new
                {
                    RelationshipInstanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelationshipId = table.Column<int>(type: "int", nullable: false),
                    SourceInstanceId = table.Column<int>(type: "int", nullable: false),
                    TargetInstanceId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomObjectRelationshipInstances", x => x.RelationshipInstanceId);
                    table.ForeignKey(
                        name: "FK_CustomObjectRelationshipInstances_CustomObjectInstances_SourceInstanceId",
                        column: x => x.SourceInstanceId,
                        principalTable: "CustomObjectInstances",
                        principalColumn: "InstanceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomObjectRelationshipInstances_CustomObjectInstances_TargetInstanceId",
                        column: x => x.TargetInstanceId,
                        principalTable: "CustomObjectInstances",
                        principalColumn: "InstanceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomObjectRelationshipInstances_CustomObjectRelationships_RelationshipId",
                        column: x => x.RelationshipId,
                        principalTable: "CustomObjectRelationships",
                        principalColumn: "RelationshipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomObjectFields_CustomObjectId",
                table: "CustomObjectFields",
                column: "CustomObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomObjectFields_LookupObjectId",
                table: "CustomObjectFields",
                column: "LookupObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomObjectFieldValues_FieldId",
                table: "CustomObjectFieldValues",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomObjectFieldValues_InstanceId",
                table: "CustomObjectFieldValues",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomObjectInstances_CustomObjectId",
                table: "CustomObjectInstances",
                column: "CustomObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomObjectRelationshipInstances_RelationshipId",
                table: "CustomObjectRelationshipInstances",
                column: "RelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomObjectRelationshipInstances_SourceInstanceId",
                table: "CustomObjectRelationshipInstances",
                column: "SourceInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomObjectRelationshipInstances_TargetInstanceId",
                table: "CustomObjectRelationshipInstances",
                column: "TargetInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomObjectRelationships_SourceObjectId",
                table: "CustomObjectRelationships",
                column: "SourceObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomObjectRelationships_TargetObjectId",
                table: "CustomObjectRelationships",
                column: "TargetObjectId");
        }
    }
}
