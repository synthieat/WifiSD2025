using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Rating",
                table: "Movies",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("15614c54-4523-4ad0-87bf-8579f38f793b"),
                column: "Rating",
                value: (byte)20);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("92436ae8-7c68-482a-9230-f289e53e1156"),
                column: "Rating",
                value: (byte)30);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("b4aa56ae-dc15-41dd-988f-dbaa5d0f510e"),
                column: "Rating",
                value: (byte)10);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("eeb15090-d5a6-46b9-a1a2-59eafb99ad24"),
                column: "Rating",
                value: (byte)20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Movies");
        }
    }
}
