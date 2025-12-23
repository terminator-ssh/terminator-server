using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Terminator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    KeySalt = table.Column<byte[]>(type: "BLOB", nullable: false),
                    AuthSalt = table.Column<byte[]>(type: "BLOB", nullable: false),
                    EncryptedMasterKey = table.Column<byte[]>(type: "BLOB", nullable: false),
                    LoginHash = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EncryptedBlobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    InitializationVector = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Blob = table.Column<byte[]>(type: "BLOB", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncryptedBlobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EncryptedBlobs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EncryptedBlobs_UserId",
                table: "EncryptedBlobs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EncryptedBlobs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
