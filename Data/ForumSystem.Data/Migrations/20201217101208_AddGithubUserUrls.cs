using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSystem.Data.Migrations
{
    public partial class AddGithubUserUrls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GithubUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GithubUrl",
                table: "AspNetUsers");
        }
    }
}
