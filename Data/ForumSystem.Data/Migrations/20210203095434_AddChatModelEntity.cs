using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSystem.Data.Migrations
{
    public partial class AddChatModelEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConversationId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuyerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SellerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsReadByBuyer = table.Column<bool>(type: "bit", nullable: false),
                    IsReadBySeller = table.Column<bool>(type: "bit", nullable: false),
                    IsArchivedByBuyer = table.Column<bool>(type: "bit", nullable: false),
                    IsArchivedBySeller = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessage",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SendOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ConversationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessage_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessage_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessage_Conversation_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ConversationId",
                table: "AspNetUsers",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ConversationId",
                table: "ChatMessage",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_IsDeleted",
                table: "ChatMessage",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ReceiverId",
                table: "ChatMessage",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_SenderId",
                table: "ChatMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_IsDeleted",
                table: "Conversation",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Conversation_ConversationId",
                table: "AspNetUsers",
                column: "ConversationId",
                principalTable: "Conversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Conversation_ConversationId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ChatMessage");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ConversationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "AspNetUsers");
        }
    }
}
