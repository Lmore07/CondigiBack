using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CondigiBack.Migrations
{
    /// <inheritdoc />
    public partial class ai_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contract_participants_contracts_ContracId",
                table: "contract_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_contracts_contract_types_ContractTypeId",
                table: "contracts");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "contracts",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "contracts",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "contracts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "contracts",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "contracts",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "contracts",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "PaymentFrequency",
                table: "contracts",
                newName: "payment_frequency");

            migrationBuilder.RenameColumn(
                name: "NumClauses",
                table: "contracts",
                newName: "num_clauses");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "contracts",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "EncryptionKey",
                table: "contracts",
                newName: "encryption_key");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "contracts",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "contracts",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ContractTypeId",
                table: "contracts",
                newName: "contract_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_contracts_ContractTypeId",
                table: "contracts",
                newName: "IX_contracts_contract_type_id");

            migrationBuilder.RenameColumn(
                name: "ContracId",
                table: "contract_participants",
                newName: "ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_contract_participants_ContracId",
                table: "contract_participants",
                newName: "IX_contract_participants_ContractId");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "contracts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "Signed",
                table: "contract_participants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ai_requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ContractId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_requests_contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "contracts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ai_requests_ContractId",
                table: "ai_requests",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_contract_participants_contracts_ContractId",
                table: "contract_participants",
                column: "ContractId",
                principalTable: "contracts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_contracts_contract_types_contract_type_id",
                table: "contracts",
                column: "contract_type_id",
                principalTable: "contract_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contract_participants_contracts_ContractId",
                table: "contract_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_contracts_contract_types_contract_type_id",
                table: "contracts");

            migrationBuilder.DropTable(
                name: "ai_requests");

            migrationBuilder.DropColumn(
                name: "Signed",
                table: "contract_participants");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "contracts",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "contracts",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "contracts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "contracts",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "contracts",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "contracts",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "payment_frequency",
                table: "contracts",
                newName: "PaymentFrequency");

            migrationBuilder.RenameColumn(
                name: "num_clauses",
                table: "contracts",
                newName: "NumClauses");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "contracts",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "encryption_key",
                table: "contracts",
                newName: "EncryptionKey");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "contracts",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "contracts",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "contract_type_id",
                table: "contracts",
                newName: "ContractTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_contracts_contract_type_id",
                table: "contracts",
                newName: "IX_contracts_ContractTypeId");

            migrationBuilder.RenameColumn(
                name: "ContractId",
                table: "contract_participants",
                newName: "ContracId");

            migrationBuilder.RenameIndex(
                name: "IX_contract_participants_ContractId",
                table: "contract_participants",
                newName: "IX_contract_participants_ContracId");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "contracts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_contract_participants_contracts_ContracId",
                table: "contract_participants",
                column: "ContracId",
                principalTable: "contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_contracts_contract_types_ContractTypeId",
                table: "contracts",
                column: "ContractTypeId",
                principalTable: "contract_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
