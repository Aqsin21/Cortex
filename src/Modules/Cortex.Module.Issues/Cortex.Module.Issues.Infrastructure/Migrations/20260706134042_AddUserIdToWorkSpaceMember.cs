using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cortex.Module.Issues.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToWorkSpaceMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WorkSpaceMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkSpaceMembers");
        }
    }
}
