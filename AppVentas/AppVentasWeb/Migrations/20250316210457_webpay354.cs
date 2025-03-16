using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppVentasWeb.Migrations
{
    /// <inheritdoc />
    public partial class webpay354 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "WebpayRests",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "WebpayRests");
        }
    }
}
