using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HammerDrop_Auction_app.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBidAndAdModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProxyMaxBid",
                table: "Ads");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxAutoBid",
                table: "Bids",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Bids",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserAccountId",
                table: "Ads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ads_UserAccountId",
                table: "Ads",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_UserAccounts_UserAccountId",
                table: "Ads",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_UserAccounts_UserAccountId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_UserAccountId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "MaxAutoBid",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "Ads");

            migrationBuilder.AddColumn<decimal>(
                name: "ProxyMaxBid",
                table: "Ads",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
