using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HammerDrop_Auction_app.Migrations
{
    /// <inheritdoc />
    public partial class AddAdImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SubcategoryId = table.Column<int>(type: "int", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MobilePhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowPhoneNumberInAds = table.Column<bool>(type: "bit", nullable: false),
                    IsAuction = table.Column<bool>(type: "bit", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReservedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinimumBidIncrement = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentHighestBid = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AuctionEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnableProxyBidding = table.Column<bool>(type: "bit", nullable: false),
                    ProxyMaxBid = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AuctionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ads_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ads_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ads_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ads_Subcategories_SubcategoryId",
                        column: x => x.SubcategoryId,
                        principalTable: "Subcategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdImagess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdImagess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdImagess_Ads_AdId",
                        column: x => x.AdId,
                        principalTable: "Ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdImagess_AdId",
                table: "AdImagess",
                column: "AdId");

            migrationBuilder.CreateIndex(
                name: "IX_Ads_CityId",
                table: "Ads",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Ads_CountryId",
                table: "Ads",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ads_StateId",
                table: "Ads",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Ads_SubcategoryId",
                table: "Ads",
                column: "SubcategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdImagess");

            migrationBuilder.DropTable(
                name: "Ads");
        }
    }
}
