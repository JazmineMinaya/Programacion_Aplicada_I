using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroEstudiantes.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDevolucionPrestamos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Devuelto",
                table: "Prestamos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDevolucion",
                table: "Prestamos",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Devuelto",
                table: "Prestamos");

            migrationBuilder.DropColumn(
                name: "FechaDevolucion",
                table: "Prestamos");
        }
    }
}
