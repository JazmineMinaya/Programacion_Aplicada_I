using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroEstudiantes.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCantidadDisponible : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Devuelto",
                table: "Prestamos");

            migrationBuilder.AddColumn<int>(
                name: "CantidadDisponible",
                table: "Libros",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadDisponible",
                table: "Libros");

            migrationBuilder.AddColumn<bool>(
                name: "Devuelto",
                table: "Prestamos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
