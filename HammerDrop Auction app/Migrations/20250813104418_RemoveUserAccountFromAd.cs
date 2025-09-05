using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HammerDrop_Auction_app.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserAccountFromAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_UserAccounts_UserAccountId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_UserAccountId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "Ads");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
