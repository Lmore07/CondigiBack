using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CondigiBack.Migrations
{
    /// <inheritdoc />
    public partial class editbd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add a new temporary column
            migrationBuilder.AddColumn<int>(
                name: "RoleInCompanyTemp",
                table: "user_companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // Step 2: Copy data from old column to new column
            migrationBuilder.Sql("UPDATE user_companies SET \"RoleInCompanyTemp\" = CAST(\"RoleInCompany\" AS integer)");

            // Step 3: Drop the old column
            migrationBuilder.DropColumn(
                name: "RoleInCompany",
                table: "user_companies");

            // Step 4: Rename the new column to the old column name
            migrationBuilder.RenameColumn(
                name: "RoleInCompanyTemp",
                table: "user_companies",
                newName: "RoleInCompany");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add the old column back
            migrationBuilder.AddColumn<string>(
                name: "RoleInCompanyTemp",
                table: "user_companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            // Step 2: Copy data from new column to old column
            migrationBuilder.Sql("UPDATE user_companies SET \"RoleInCompanyTemp\" = CAST(\"RoleInCompany\" AS text)");

            // Step 3: Drop the new column
            migrationBuilder.DropColumn(
                name: "RoleInCompany",
                table: "user_companies");

            // Step 4: Rename the old column back to the original name
            migrationBuilder.RenameColumn(
                name: "RoleInCompanyTemp",
                table: "user_companies",
                newName: "RoleInCompany");
        }
    }
}