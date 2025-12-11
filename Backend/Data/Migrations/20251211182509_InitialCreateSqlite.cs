using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Temak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cim = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temak", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kommentek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TemaId = table.Column<int>(type: "INTEGER", nullable: false),
                    FelhasznaloNev = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Szoveg = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kommentek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kommentek_Temak_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kommentek_TemaId",
                table: "Kommentek",
                column: "TemaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kommentek");

            migrationBuilder.DropTable(
                name: "Temak");
        }
    }
}
