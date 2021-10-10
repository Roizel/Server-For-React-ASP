using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Cars.Migrations
{
    public partial class AddFIOtousers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FIO",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FIO",
                table: "AspNetUsers");
        }
    }
}
