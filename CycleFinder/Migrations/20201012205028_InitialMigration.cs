using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CycleFinder.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ephemeris",
                columns: table => new
                {
                    Date = table.Column<DateTime>(nullable: false),
                    Moon_Longitude = table.Column<double>(nullable: true),
                    Moon_Latitude = table.Column<double>(nullable: true),
                    Moon_Declination = table.Column<double>(nullable: true),
                    Moon_Speed = table.Column<double>(nullable: true),
                    Sun_Longitude = table.Column<double>(nullable: true),
                    Sun_Latitude = table.Column<double>(nullable: true),
                    Sun_Declination = table.Column<double>(nullable: true),
                    Sun_Speed = table.Column<double>(nullable: true),
                    Mercury_Longitude = table.Column<double>(nullable: true),
                    Mercury_Latitude = table.Column<double>(nullable: true),
                    Mercury_Declination = table.Column<double>(nullable: true),
                    Mercury_Speed = table.Column<double>(nullable: true),
                    Venus_Longitude = table.Column<double>(nullable: true),
                    Venus_Latitude = table.Column<double>(nullable: true),
                    Venus_Declination = table.Column<double>(nullable: true),
                    Venus_Speed = table.Column<double>(nullable: true),
                    Mars_Longitude = table.Column<double>(nullable: true),
                    Mars_Latitude = table.Column<double>(nullable: true),
                    Mars_Declination = table.Column<double>(nullable: true),
                    Mars_Speed = table.Column<double>(nullable: true),
                    Jupiter_Longitude = table.Column<double>(nullable: true),
                    Jupiter_Latitude = table.Column<double>(nullable: true),
                    Jupiter_Declination = table.Column<double>(nullable: true),
                    Jupiter_Speed = table.Column<double>(nullable: true),
                    Saturn_Longitude = table.Column<double>(nullable: true),
                    Saturn_Latitude = table.Column<double>(nullable: true),
                    Saturn_Declination = table.Column<double>(nullable: true),
                    Saturn_Speed = table.Column<double>(nullable: true),
                    Uranus_Longitude = table.Column<double>(nullable: true),
                    Uranus_Latitude = table.Column<double>(nullable: true),
                    Uranus_Declination = table.Column<double>(nullable: true),
                    Uranus_Speed = table.Column<double>(nullable: true),
                    Neptune_Longitude = table.Column<double>(nullable: true),
                    Neptune_Latitude = table.Column<double>(nullable: true),
                    Neptune_Declination = table.Column<double>(nullable: true),
                    Neptune_Speed = table.Column<double>(nullable: true),
                    Pluto_Longitude = table.Column<double>(nullable: true),
                    Pluto_Latitude = table.Column<double>(nullable: true),
                    Pluto_Declination = table.Column<double>(nullable: true),
                    Pluto_Speed = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ephemeris", x => x.Date);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ephemeris");
        }
    }
}
