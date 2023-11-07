using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaVeterinaria.Migrations
{
    /// <inheritdoc />
    public partial class Teste6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_agendamentos_clientes_idCliente",
                table: "agendamentos");

            migrationBuilder.AlterColumn<string>(
                name: "nomeAnimal",
                table: "agendamentos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "idCliente",
                table: "agendamentos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "especie",
                table: "agendamentos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_agendamentos_clientes_idCliente",
                table: "agendamentos",
                column: "idCliente",
                principalTable: "clientes",
                principalColumn: "idCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_agendamentos_clientes_idCliente",
                table: "agendamentos");

            migrationBuilder.AlterColumn<string>(
                name: "nomeAnimal",
                table: "agendamentos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "idCliente",
                table: "agendamentos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "especie",
                table: "agendamentos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_agendamentos_clientes_idCliente",
                table: "agendamentos",
                column: "idCliente",
                principalTable: "clientes",
                principalColumn: "idCliente",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
