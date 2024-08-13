using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CondigiBack.Migrations
{
    /// <inheritdoc />
    public partial class contract_participant_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contract_participants_companies_CompanyId",
                table: "contract_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_contract_participants_contracts_ContractId",
                table: "contract_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_contract_participants_users_UserId",
                table: "contract_participants");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "contract_participants",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Signed",
                table: "contract_participants",
                newName: "signed");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "contract_participants",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "contract_participants",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "contract_participants",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ContractId",
                table: "contract_participants",
                newName: "contract_id");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "contract_participants",
                newName: "company_id");

            migrationBuilder.RenameIndex(
                name: "IX_contract_participants_UserId",
                table: "contract_participants",
                newName: "IX_contract_participants_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_contract_participants_ContractId",
                table: "contract_participants",
                newName: "IX_contract_participants_contract_id");

            migrationBuilder.RenameIndex(
                name: "IX_contract_participants_CompanyId",
                table: "contract_participants",
                newName: "IX_contract_participants_company_id");

            migrationBuilder.DropColumn(
                name: "status",
                table: "contract_participants");

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "contract_participants",
                type: "boolean",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_contract_participants_companies_company_id",
                table: "contract_participants",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_contract_participants_contracts_contract_id",
                table: "contract_participants",
                column: "contract_id",
                principalTable: "contracts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_contract_participants_users_user_id",
                table: "contract_participants",
                column: "user_id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contract_participants_companies_company_id",
                table: "contract_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_contract_participants_contracts_contract_id",
                table: "contract_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_contract_participants_users_user_id",
                table: "contract_participants");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "contract_participants",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "signed",
                table: "contract_participants",
                newName: "Signed");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "contract_participants",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "contract_participants",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "contract_participants",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "contract_id",
                table: "contract_participants",
                newName: "ContractId");

            migrationBuilder.RenameColumn(
                name: "company_id",
                table: "contract_participants",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_contract_participants_user_id",
                table: "contract_participants",
                newName: "IX_contract_participants_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_contract_participants_contract_id",
                table: "contract_participants",
                newName: "IX_contract_participants_ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_contract_participants_company_id",
                table: "contract_participants",
                newName: "IX_contract_participants_CompanyId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "contract_participants",
                type: "text",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddForeignKey(
                name: "FK_contract_participants_companies_CompanyId",
                table: "contract_participants",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_contract_participants_contracts_ContractId",
                table: "contract_participants",
                column: "ContractId",
                principalTable: "contracts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_contract_participants_users_UserId",
                table: "contract_participants",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
