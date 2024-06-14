using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS_Web_Viec_Lam.Migrations
{
    /// <inheritdoc />
    public partial class test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "notifications");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobseekerId",
                table: "notifications",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "JobseekerId",
                table: "notifications");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
