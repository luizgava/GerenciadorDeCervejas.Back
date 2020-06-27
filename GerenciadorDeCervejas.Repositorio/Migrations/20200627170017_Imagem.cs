using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GerenciadorDeCervejas.Repositorio.Migrations
{
    public partial class Imagem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cervejas_imagens",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    datacriacao = table.Column<DateTime>(nullable: false),
                    dataalteracao = table.Column<DateTime>(nullable: false),
                    nomearquivo = table.Column<string>(maxLength: 100, nullable: false),
                    contenttype = table.Column<string>(maxLength: 100, nullable: false),
                    bytes = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cervejas_imagens", x => x.id);
                    table.ForeignKey(
                        name: "FK_cervejas_imagens_cervejas_id",
                        column: x => x.id,
                        principalTable: "cervejas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cervejas_imagens");
        }
    }
}
