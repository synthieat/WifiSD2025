using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SD.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Intial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediumTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediumTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    MediumTypeCode = table.Column<string>(type: "nvarchar(8)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movies_MediumTypes_MediumTypeCode",
                        column: x => x.MediumTypeCode,
                        principalTable: "MediumTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Horror" },
                    { 3, "Science Fiction" },
                    { 4, "Comedy" }
                });

            migrationBuilder.InsertData(
                table: "MediumTypes",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { "BR", "Blu-ray Disc" },
                    { "BR3D", "3D Blu-ray Disc" },
                    { "BR4K", "4K Blu-ray Disc" },
                    { "DVD", "Digital Versatile Disc" },
                    { "VHS", "Video Home System" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "GenreId", "MediumTypeCode", "Price", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("15614c54-4523-4ad0-87bf-8579f38f793b"), 3, "DVD", 9.9m, new DateTime(1987, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Star Wars - Episode IV" },
                    { new Guid("92436ae8-7c68-482a-9230-f289e53e1156"), 3, "BR3D", 14.9m, new DateTime(2016, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Star Trek - Beyond" },
                    { new Guid("b4aa56ae-dc15-41dd-988f-dbaa5d0f510e"), 2, "BR", 12.9m, new DateTime(2005, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Ring" },
                    { new Guid("eeb15090-d5a6-46b9-a1a2-59eafb99ad24"), 1, "VHS", 4.9m, new DateTime(1985, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rambo" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GenreId",
                table: "Movies",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_MediumTypeCode",
                table: "Movies",
                column: "MediumTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Title",
                table: "Movies",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "MediumTypes");
        }
    }
}
