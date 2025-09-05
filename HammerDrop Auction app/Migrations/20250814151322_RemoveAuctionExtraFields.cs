using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HammerDrop_Auction_app.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAuctionExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxAutoBid",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "AuctionStatus",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "CurrentHighestBid",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "EnableProxyBidding",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "MinimumBidIncrement",
                table: "Ads");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxAutoBid",
                table: "Bids",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuctionStatus",
                table: "Ads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentHighestBid",
                table: "Ads",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableProxyBidding",
                table: "Ads",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumBidIncrement",
                table: "Ads",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
