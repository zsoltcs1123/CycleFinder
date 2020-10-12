using Microsoft.EntityFrameworkCore.Migrations;

namespace CycleFinder.Migrations
{
    public partial class UpdateNames01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ephemeris",
                table: "Ephemeris");

            migrationBuilder.RenameTable(
                name: "Ephemeris",
                newName: "EphemerisEntries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EphemerisEntries",
                table: "EphemerisEntries",
                column: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EphemerisEntries",
                table: "EphemerisEntries");

            migrationBuilder.RenameTable(
                name: "EphemerisEntries",
                newName: "Ephemeris");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ephemeris",
                table: "Ephemeris",
                column: "Date");
        }
    }
}
