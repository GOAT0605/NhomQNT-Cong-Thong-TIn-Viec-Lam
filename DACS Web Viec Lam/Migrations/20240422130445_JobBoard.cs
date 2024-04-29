using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS_Web_Viec_Lam.Migrations
{
    /// <inheritdoc />
    public partial class JobBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "JobSeeker",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Employers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeeker_userId",
                table: "JobSeeker",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Employers_userId",
                table: "Employers",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employers_AspNetUsers_userId",
                table: "Employers",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeeker_AspNetUsers_userId",
                table: "JobSeeker",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employers_AspNetUsers_userId",
                table: "Employers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeeker_AspNetUsers_userId",
                table: "JobSeeker");

            migrationBuilder.DropIndex(
                name: "IX_JobSeeker_userId",
                table: "JobSeeker");

            migrationBuilder.DropIndex(
                name: "IX_Employers_userId",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "JobSeeker");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Employers");
        }
    }
}
