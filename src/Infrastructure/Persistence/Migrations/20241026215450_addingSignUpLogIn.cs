using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addingSignUpLogIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "user",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "user",
                newName: "email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "user",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "user",
                newName: "description");
        }
    }
}
