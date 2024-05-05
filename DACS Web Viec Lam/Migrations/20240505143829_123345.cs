using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS_Web_Viec_Lam.Migrations
{
    /// <inheritdoc />
    public partial class _123345 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "JobSeeker",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobSeekerId",
                table: "EmployerImage",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployerImage_JobSeekerId",
                table: "EmployerImage",
                column: "JobSeekerId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerImage_JobSeeker_JobSeekerId",
                table: "EmployerImage",
                column: "JobSeekerId",
                principalTable: "JobSeeker",
                principalColumn: "JobSeekerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerImage_JobSeeker_JobSeekerId",
                table: "EmployerImage");

            migrationBuilder.DropIndex(
                name: "IX_EmployerImage_JobSeekerId",
                table: "EmployerImage");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "JobSeeker");

            migrationBuilder.DropColumn(
                name: "JobSeekerId",
                table: "EmployerImage");
        }
    }
}
