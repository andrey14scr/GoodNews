using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodNewsAggregator.DAL.Core.Migrations
{
    public partial class ChangeGoodFactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GoodFactor",
                table: "Articles",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "GoodFactor",
                table: "Articles",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
