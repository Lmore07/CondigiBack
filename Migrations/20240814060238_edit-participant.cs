using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CondigiBack.Migrations
{
    /// <inheritdoc />
    public partial class editparticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contract_participants_users_user_id",
                table: "contract_participants");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "companies");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "contract_participants",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_contract_participants_users_user_id",
                table: "contract_participants",
                column: "user_id",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contract_participants_users_user_id",
                table: "contract_participants");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "contract_participants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "companies",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_contract_participants_users_user_id",
                table: "contract_participants",
                column: "user_id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
