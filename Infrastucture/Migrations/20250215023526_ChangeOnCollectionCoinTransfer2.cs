using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOnCollectionCoinTransfer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoinTransfers_Users_FromUserId",
                table: "CoinTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_CoinTransfers_Users_ToUserId",
                table: "CoinTransfers");

            migrationBuilder.AddForeignKey(
                name: "FK_CoinTransfers_Users_FromUserId",
                table: "CoinTransfers",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CoinTransfers_Users_ToUserId",
                table: "CoinTransfers",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoinTransfers_Users_FromUserId",
                table: "CoinTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_CoinTransfers_Users_ToUserId",
                table: "CoinTransfers");

            migrationBuilder.AddForeignKey(
                name: "FK_CoinTransfers_Users_FromUserId",
                table: "CoinTransfers",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoinTransfers_Users_ToUserId",
                table: "CoinTransfers",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
