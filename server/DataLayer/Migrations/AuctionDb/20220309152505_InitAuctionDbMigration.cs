using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations.AuctionDb
{
    public partial class InitAuctionDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageFileName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<int>(type: "int", nullable: false),
                    SellerPrice = table.Column<int>(type: "int", nullable: false),
                    ImageFileName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BidderEmail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ImageFileName", "Name" },
                values: new object[] { 1L, "7a9ae808-5cba-4aca-a548-9e5497dcf520.png", "Monitors" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ImageFileName", "Name" },
                values: new object[] { 2L, "d9b53dda-f99b-4569-a294-3807faefec49.png", "Smartphones" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ImageFileName", "Name" },
                values: new object[] { 3L, "f2647ff0-3530-4e7a-a009-a2957e19db6d.png", "Tablets" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BidderEmail", "CategoryId", "Description", "ImageFileName", "Name", "Price", "SellerPrice" },
                values: new object[,]
                {
                    { 1L, null, 1L, "M: 1201", "7b739908-b892-4961-80cd-46a80d45b1e9.png", "Monitor", 500000, 500000 },
                    { 4L, null, 1L, "M: 1202", "9d3ace22-2a12-4fe8-8998-a4464aff13f6.png", "Monitor", 500000, 500000 },
                    { 7L, null, 1L, "M: 1203", "578dd49b-b719-40bb-a562-31f47f98c68b.png", "Monitor", 500000, 500000 },
                    { 10L, null, 1L, "M: 1204", "9049d1b8-2d9a-40f7-b38f-b5d185d4b547.png", "Monitor", 500000, 500000 },
                    { 13L, null, 1L, "M: 1205", "99145066-ab8a-409f-88a9-6096ce62d6f6.png", "Monitor", 500000, 500000 },
                    { 2L, null, 2L, "S: 1301", "0cbc67c4-37e1-4046-9068-5fd6b5eb8410.png", "Smartphone", 300000, 300000 },
                    { 5L, null, 2L, "S: 1302", "2f4e71c1-b99f-41f5-a4e3-04eae3998cf4.png", "Smartphone", 300000, 300000 },
                    { 8L, null, 2L, "S: 1303", "7b5554f9-8eee-4d7f-95ed-943b31ddfcfd.png", "Smartphone", 300000, 300000 },
                    { 11L, null, 2L, "S: 1304", "56c9618e-fb42-42d3-bdc3-eeec29f28e72.png", "Smartphone", 300000, 300000 },
                    { 14L, null, 2L, "S: 1305", "60917223-b1cb-47a0-a167-b229d2bca970.png", "Smartphone", 300000, 300000 },
                    { 3L, null, 3L, "T: 1401", "00c24b2c-3117-4e82-8161-c877e8550b09.png", "Tablet", 400000, 400000 },
                    { 6L, null, 3L, "T: 1402", "1a710d16-e145-4eff-9df3-8422e917e582.png", "Tablet", 400000, 400000 },
                    { 9L, null, 3L, "T: 1403", "7eb25132-59a7-4ce8-95a4-fb9239200147.png", "Tablet", 400000, 400000 },
                    { 12L, null, 3L, "T: 1404", "babeb68c-ff0b-4b24-b237-8c57c4773924.png", "Tablet", 400000, 400000 },
                    { 15L, null, 3L, "T: 1405", "d84d23da-f2ca-4271-8a49-b4d0b474d549.png", "Tablet", 400000, 400000 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
