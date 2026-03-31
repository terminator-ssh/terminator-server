using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Terminator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserMetrics",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstSyncAt = table.Column<long>(type: "INTEGER", nullable: false),
                    LastSyncAt = table.Column<long>(type: "INTEGER", nullable: false),
                    SyncCount = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMetrics", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserMetrics_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMetrics");
        }
    }
}
