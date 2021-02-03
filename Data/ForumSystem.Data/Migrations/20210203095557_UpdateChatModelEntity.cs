using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSystem.Data.Migrations
{
    public partial class UpdateChatModelEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchivedByBuyer",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "IsArchivedBySeller",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "IsReadByBuyer",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "IsReadBySeller",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "StartedOn",
                table: "Conversation");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Conversation",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Conversation",
                newName: "ReceiverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Conversation",
                newName: "SellerId");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Conversation",
                newName: "BuyerId");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchivedByBuyer",
                table: "Conversation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchivedBySeller",
                table: "Conversation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadByBuyer",
                table: "Conversation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadBySeller",
                table: "Conversation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedOn",
                table: "Conversation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
