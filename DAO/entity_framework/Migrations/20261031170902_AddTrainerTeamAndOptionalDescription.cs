using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.entity_framework.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainerTeamAndOptionalDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TeamId",
                table: "Trainers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Activities",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_TeamId",
                table: "Trainers",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Teams_TeamId",
                table: "Trainers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Teams_TeamId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_TeamId",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Trainers");

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Description",
                keyValue: null,
                column: "Description",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Activities",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
