using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class renametablesname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Employees_NIK",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_University_University_Id",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiling_Account_NIK",
                table: "Profiling");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiling_Education_Education_id",
                table: "Profiling");

            migrationBuilder.DropPrimaryKey(
                name: "PK_University",
                table: "University");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profiling",
                table: "Profiling");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Education",
                table: "Education");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "University",
                newName: "Universities");

            migrationBuilder.RenameTable(
                name: "Profiling",
                newName: "Profilings");

            migrationBuilder.RenameTable(
                name: "Education",
                newName: "Educations");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_Profiling_Education_id",
                table: "Profilings",
                newName: "IX_Profilings_Education_id");

            migrationBuilder.RenameIndex(
                name: "IX_Education_University_Id",
                table: "Educations",
                newName: "IX_Educations_University_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Universities",
                table: "Universities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profilings",
                table: "Profilings",
                column: "NIK");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Educations",
                table: "Educations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "NIK");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Employees_NIK",
                table: "Accounts",
                column: "NIK",
                principalTable: "Employees",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Universities_University_Id",
                table: "Educations",
                column: "University_Id",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profilings_Accounts_NIK",
                table: "Profilings",
                column: "NIK",
                principalTable: "Accounts",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profilings_Educations_Education_id",
                table: "Profilings",
                column: "Education_id",
                principalTable: "Educations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Employees_NIK",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Universities_University_Id",
                table: "Educations");

            migrationBuilder.DropForeignKey(
                name: "FK_Profilings_Accounts_NIK",
                table: "Profilings");

            migrationBuilder.DropForeignKey(
                name: "FK_Profilings_Educations_Education_id",
                table: "Profilings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Universities",
                table: "Universities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profilings",
                table: "Profilings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Educations",
                table: "Educations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Universities",
                newName: "University");

            migrationBuilder.RenameTable(
                name: "Profilings",
                newName: "Profiling");

            migrationBuilder.RenameTable(
                name: "Educations",
                newName: "Education");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Account");

            migrationBuilder.RenameIndex(
                name: "IX_Profilings_Education_id",
                table: "Profiling",
                newName: "IX_Profiling_Education_id");

            migrationBuilder.RenameIndex(
                name: "IX_Educations_University_Id",
                table: "Education",
                newName: "IX_Education_University_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_University",
                table: "University",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profiling",
                table: "Profiling",
                column: "NIK");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Education",
                table: "Education",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "NIK");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Employees_NIK",
                table: "Account",
                column: "NIK",
                principalTable: "Employees",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_University_University_Id",
                table: "Education",
                column: "University_Id",
                principalTable: "University",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiling_Account_NIK",
                table: "Profiling",
                column: "NIK",
                principalTable: "Account",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiling_Education_Education_id",
                table: "Profiling",
                column: "Education_id",
                principalTable: "Education",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
