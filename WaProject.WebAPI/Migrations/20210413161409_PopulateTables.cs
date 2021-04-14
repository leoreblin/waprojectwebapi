using Microsoft.EntityFrameworkCore.Migrations;

namespace WaProject.WebAPI.Migrations
{
    public partial class PopulateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
@"INSERT INTO Equipes(Nome, Descricao, PlacaVeiculo)
VALUES('Equipe A', 'Equipe de vendas de camisetas', 'AAA-2910')");
            migrationBuilder.Sql(
 @"INSERT INTO Equipes(Nome, Descricao, PlacaVeiculo)
VALUES('Equipe B', 'Equipe de vendas de calçados', 'BBB-3010')");
            migrationBuilder.Sql(
 @"INSERT INTO Equipes(Nome, Descricao, PlacaVeiculo)
VALUES('Equipe C', 'Equipe de vendas de perfumes', 'CCC-3110')");

            migrationBuilder.Sql(
 @"INSERT INTO Produtos(Nome, Descricao, Valor)
VALUES('Camiseta Básica Preta', 'Camiseta sem estampa cor preta', 49.90)");
            migrationBuilder.Sql(
@"INSERT INTO Produtos(Nome, Descricao, Valor)
VALUES('Camiseta Básica Branca', 'Camiseta sem estampa cor branca', 49.90)");
            migrationBuilder.Sql(
@"INSERT INTO Produtos(Nome, Descricao, Valor)
VALUES('Camiseta WaProject', 'Camiseta com estampa da WaProject', 59.90)");
            migrationBuilder.Sql(
 @"INSERT INTO Produtos(Nome, Descricao, Valor)
VALUES('Sapato Mocassim', 'Sapato estilo mocassim cor marrom', 219.90)");
            migrationBuilder.Sql(
 @"INSERT INTO Produtos(Nome, Descricao, Valor)
VALUES('Tênis', 'Tênis para corrida cor preto', 319.90)");
            migrationBuilder.Sql(
 @"INSERT INTO Produtos(Nome, Descricao, Valor)
VALUES('Perfume Masculino', 'Perfume para homens Eau de Toillet', 519.90)");
            migrationBuilder.Sql(
 @"INSERT INTO Produtos(Nome, Descricao, Valor)
VALUES('Perfume Feminino', 'Perfume para mulheres Eau de Toillet', 519.90)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
