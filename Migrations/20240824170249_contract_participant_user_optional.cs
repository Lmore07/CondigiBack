using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CondigiBack.Migrations
{
    /// <inheritdoc />
    public partial class contract_participant_user_optional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            // columna user_id en contract_participants ahora es nullable
            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "contract_participants",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // columna user_id en contract_participants ahora no es nullable
            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "contract_participants",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
