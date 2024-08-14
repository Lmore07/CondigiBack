using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CondigiBack.Migrations
{
    /// <inheritdoc />
    public partial class editcompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ParishId",
                table: "companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RUC",
                table: "companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_companies_ParishId",
                table: "companies",
                column: "ParishId");

            migrationBuilder.CreateIndex(
                name: "IX_companies_RUC",
                table: "companies",
                column: "RUC",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_companies_parishes_ParishId",
                table: "companies",
                column: "ParishId",
                principalTable: "parishes",
                principalColumn: "IdParish",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_companies_parishes_ParishId",
                table: "companies");

            migrationBuilder.DropIndex(
                name: "IX_companies_ParishId",
                table: "companies");

            migrationBuilder.DropIndex(
                name: "IX_companies_RUC",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "ParishId",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "RUC",
                table: "companies");
        }
    }
}
