using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hcom.Web.Api.Migrations.FileUploadDB
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConstructionMilestoneBinaryImages",
                columns: table => new
                {
                    cmiFileID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    cmiFileBinary = table.Column<byte[]>(nullable: true),
                    cmiFileName = table.Column<string>(nullable: true),
                    cmiDateCreated = table.Column<DateTime>(nullable: false),
                    cmiCreatedBy = table.Column<string>(nullable: true),
                    cmiDateModified = table.Column<DateTime>(nullable: true),
                    cmiModifiedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionMilestoneBinaryImages", x => x.cmiFileID);
                });

            migrationBuilder.CreateTable(
                name: "PunchListBinaryImages",
                columns: table => new
                {
                    cimFileID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    cimFileName = table.Column<string>(nullable: true),
                    cimFileBinary = table.Column<byte[]>(nullable: true),
                    cimDateCreated = table.Column<DateTime>(nullable: false),
                    cimCreatedBy = table.Column<string>(nullable: true),
                    cimDateModified = table.Column<DateTime>(nullable: true),
                    cimModifiedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PunchListBinaryImages", x => x.cimFileID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConstructionMilestoneBinaryImages");

            migrationBuilder.DropTable(
                name: "PunchListBinaryImages");
        }
    }
}
