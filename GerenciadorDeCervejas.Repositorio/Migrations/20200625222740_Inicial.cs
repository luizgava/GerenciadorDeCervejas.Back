using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GerenciadorDeCervejas.Repositorio.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cervejas",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    datacriacao = table.Column<DateTime>(nullable: false),
                    dataalteracao = table.Column<DateTime>(nullable: false),
                    nome = table.Column<string>(maxLength: 50, nullable: false),
                    descricao = table.Column<string>(maxLength: 1000, nullable: false),
                    harmonizacao = table.Column<string>(maxLength: 500, nullable: false),
                    cor = table.Column<string>(maxLength: 100, nullable: false),
                    teoralcoolico = table.Column<string>(maxLength: 10, nullable: false),
                    temperatura = table.Column<string>(maxLength: 10, nullable: false),
                    ingredientes = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cervejas", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cervejas");
        }
    }
}
