using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Terminator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitializationVector",
                table: "EncryptedBlobs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "InitializationVector",
                table: "EncryptedBlobs",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
