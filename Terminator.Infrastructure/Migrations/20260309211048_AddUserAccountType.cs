using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Terminator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAccountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "Full");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "Users");
        }
    }
}
