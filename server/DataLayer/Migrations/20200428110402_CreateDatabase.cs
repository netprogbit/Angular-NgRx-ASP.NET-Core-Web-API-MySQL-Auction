using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ImageFileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    SellerPrice = table.Column<int>(nullable: false),
                    ImageFileName = table.Column<string>(nullable: true),
                    Bidder = table.Column<long>(nullable: false)
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
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ImageFileName", "Name" },
                values: new object[,]
                {
                    { 1L, "7a9ae808-5cba-4aca-a548-9e5497dcf520.png", "Monitors" },
                    { 2L, "d9b53dda-f99b-4569-a294-3807faefec49.png", "Smartphones" },
                    { 3L, "f2647ff0-3530-4e7a-a009-a2957e19db6d.png", "Tablets" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role" },
                values: new object[,]
                {
                    { 1L, "admin@auction.com", "Yuri", "Yuriev", "CQF9pVh87cIuoNg0xksMsOEJrcqD86hy/H9P8fSjl8mk5ymCjBE2ZOrm1l0C6DlV5xhVeX7I9ecQ8upjo7/Dcg==", "admin" },
                    { 2L, "ivan@gmail.com", "Ivan", "Ivanov", "WgkOqhMuNVfIVBZxP++JdOWBXZkVWNbztiLBuV2ICeZxV1aDC3Rl3DHaTaDqzKdaqy0LQio+kJdy6xxDFlNR3Q==", "user" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Bidder", "CategoryId", "Description", "ImageFileName", "Name", "Price", "SellerPrice" },
                values: new object[,]
                {
                    { 1L, 0L, 1L, "M: 1201", "7b739908-b892-4961-80cd-46a80d45b1e9.png", "Monitor", 500000, 500000 },
                    { 4L, 0L, 1L, "M: 1202", "9d3ace22-2a12-4fe8-8998-a4464aff13f6.png", "Monitor", 500000, 500000 },
                    { 7L, 0L, 1L, "M: 1203", "578dd49b-b719-40bb-a562-31f47f98c68b.png", "Monitor", 500000, 500000 },
                    { 10L, 0L, 1L, "M: 1204", "9049d1b8-2d9a-40f7-b38f-b5d185d4b547.png", "Monitor", 500000, 500000 },
                    { 13L, 0L, 1L, "M: 1205", "99145066-ab8a-409f-88a9-6096ce62d6f6.png", "Monitor", 500000, 500000 },
                    { 2L, 0L, 2L, "S: 1301", "0cbc67c4-37e1-4046-9068-5fd6b5eb8410.png", "Smartphone", 300000, 300000 },
                    { 5L, 0L, 2L, "S: 1302", "2f4e71c1-b99f-41f5-a4e3-04eae3998cf4.png", "Smartphone", 300000, 300000 },
                    { 8L, 0L, 2L, "S: 1303", "7b5554f9-8eee-4d7f-95ed-943b31ddfcfd.png", "Smartphone", 300000, 300000 },
                    { 11L, 0L, 2L, "S: 1304", "56c9618e-fb42-42d3-bdc3-eeec29f28e72.png", "Smartphone", 300000, 300000 },
                    { 14L, 0L, 2L, "S: 1305", "60917223-b1cb-47a0-a167-b229d2bca970.png", "Smartphone", 300000, 300000 },
                    { 3L, 0L, 3L, "T: 1401", "00c24b2c-3117-4e82-8161-c877e8550b09.png", "Tablet", 400000, 400000 },
                    { 6L, 0L, 3L, "T: 1402", "1a710d16-e145-4eff-9df3-8422e917e582.png", "Tablet", 400000, 400000 },
                    { 9L, 0L, 3L, "T: 1403", "7eb25132-59a7-4ce8-95a4-fb9239200147.png", "Tablet", 400000, 400000 },
                    { 12L, 0L, 3L, "T: 1404", "babeb68c-ff0b-4b24-b237-8c57c4773924.png", "Tablet", 400000, 400000 },
                    { 15L, 0L, 3L, "T: 1405", "d84d23da-f2ca-4271-8a49-b4d0b474d549.png", "Tablet", 400000, 400000 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
