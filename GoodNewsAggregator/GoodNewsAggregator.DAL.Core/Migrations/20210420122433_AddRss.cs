using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodNewsAggregator.DAL.Core.Migrations
{
    public partial class AddRss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rss",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[]
                {
                    Guid.NewGuid(),
                    "Onliner.by",
                    "https://www.onliner.by/feed",
                });

            migrationBuilder.InsertData(
                table: "Rss",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[]
                {
                    Guid.NewGuid(),
                    "Tut.by",
                    "https://news.tut.by/rss/all.rss",
                });

            migrationBuilder.InsertData(
                table: "Rss",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[]
                {
                    Guid.NewGuid(),
                    "S13",
                    "http://s13.ru/rss",
                });

            migrationBuilder.InsertData(
                table: "Rss",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[]
                {
                    Guid.NewGuid(),
                    "Tjournal.ru",
                    "https://tjournal.ru/rss/",
                });

            migrationBuilder.InsertData(
                table: "Rss",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[]
                {
                    Guid.NewGuid(),
                    "Dtf.ru",
                    "https://dtf.ru/rss/",
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
