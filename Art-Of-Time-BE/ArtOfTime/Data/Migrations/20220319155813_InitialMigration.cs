using Microsoft.EntityFrameworkCore.Migrations;

namespace ArtOfTime.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    TimeStamp = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BasedOnText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFetched = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.TimeStamp);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
