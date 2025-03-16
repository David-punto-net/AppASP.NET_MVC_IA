using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppVentasWeb.Migrations
{
    /// <inheritdoc />
    public partial class webpayy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebpayRests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleId = table.Column<int>(type: "int", nullable: true),
                    Vci = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    BuyOrder = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CardDetail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AccountingDate = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorizationCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PaymentTypeCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ResponseCode = table.Column<int>(type: "int", nullable: true),
                    InstallmentsAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InstallmentsNumber = table.Column<int>(type: "int", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebpayRests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebpayRests_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebpayRests_SaleId",
                table: "WebpayRests",
                column: "SaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebpayRests");
        }
    }
}
