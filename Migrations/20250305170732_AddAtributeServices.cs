using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoStudioApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAtributeServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "Services");
        }
    }
}
