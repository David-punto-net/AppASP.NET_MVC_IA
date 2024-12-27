using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppVentasWeb.Migrations
{
    /// <inheritdoc />
    public partial class addcategoriaproductos2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Ciudades_CiudadId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Ciudades_Comunas_ComunaId",
                table: "Ciudades");

            migrationBuilder.DropForeignKey(
                name: "FK_Comunas_Regiones_RegionId",
                table: "Comunas");

            migrationBuilder.DropForeignKey(
                name: "FK_Regiones_Paises_PaisId",
                table: "Regiones");

            migrationBuilder.DropIndex(
                name: "IX_Regiones_Nombre_PaisId",
                table: "Regiones");

            migrationBuilder.DropIndex(
                name: "IX_Comunas_Nombre_RegionId",
                table: "Comunas");

            migrationBuilder.DropIndex(
                name: "IX_Ciudades_Nombre_ComunaId",
                table: "Ciudades");

            migrationBuilder.AlterColumn<int>(
                name: "PaisId",
                table: "Regiones",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Comunas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ComunaId",
                table: "Ciudades",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CiudadId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    CategoriaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCategories_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Regiones_Nombre_PaisId",
                table: "Regiones",
                columns: new[] { "Nombre", "PaisId" },
                unique: true,
                filter: "[PaisId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comunas_Nombre_RegionId",
                table: "Comunas",
                columns: new[] { "Nombre", "RegionId" },
                unique: true,
                filter: "[RegionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Ciudades_Nombre_ComunaId",
                table: "Ciudades",
                columns: new[] { "Nombre", "ComunaId" },
                unique: true,
                filter: "[ComunaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoriaId",
                table: "ProductCategories",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProductoId_CategoriaId",
                table: "ProductCategories",
                columns: new[] { "ProductoId", "CategoriaId" },
                unique: true,
                filter: "[ProductoId] IS NOT NULL AND [CategoriaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductoId",
                table: "ProductImages",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Name",
                table: "Productos",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Ciudades_CiudadId",
                table: "AspNetUsers",
                column: "CiudadId",
                principalTable: "Ciudades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ciudades_Comunas_ComunaId",
                table: "Ciudades",
                column: "ComunaId",
                principalTable: "Comunas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comunas_Regiones_RegionId",
                table: "Comunas",
                column: "RegionId",
                principalTable: "Regiones",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Regiones_Paises_PaisId",
                table: "Regiones",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Ciudades_CiudadId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Ciudades_Comunas_ComunaId",
                table: "Ciudades");

            migrationBuilder.DropForeignKey(
                name: "FK_Comunas_Regiones_RegionId",
                table: "Comunas");

            migrationBuilder.DropForeignKey(
                name: "FK_Regiones_Paises_PaisId",
                table: "Regiones");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Regiones_Nombre_PaisId",
                table: "Regiones");

            migrationBuilder.DropIndex(
                name: "IX_Comunas_Nombre_RegionId",
                table: "Comunas");

            migrationBuilder.DropIndex(
                name: "IX_Ciudades_Nombre_ComunaId",
                table: "Ciudades");

            migrationBuilder.AlterColumn<int>(
                name: "PaisId",
                table: "Regiones",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Comunas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ComunaId",
                table: "Ciudades",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CiudadId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regiones_Nombre_PaisId",
                table: "Regiones",
                columns: new[] { "Nombre", "PaisId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comunas_Nombre_RegionId",
                table: "Comunas",
                columns: new[] { "Nombre", "RegionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ciudades_Nombre_ComunaId",
                table: "Ciudades",
                columns: new[] { "Nombre", "ComunaId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Ciudades_CiudadId",
                table: "AspNetUsers",
                column: "CiudadId",
                principalTable: "Ciudades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ciudades_Comunas_ComunaId",
                table: "Ciudades",
                column: "ComunaId",
                principalTable: "Comunas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comunas_Regiones_RegionId",
                table: "Comunas",
                column: "RegionId",
                principalTable: "Regiones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Regiones_Paises_PaisId",
                table: "Regiones",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
