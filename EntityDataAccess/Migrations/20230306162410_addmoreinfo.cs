using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addmoreinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MoreInfo",
                table: "Outputs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MoreInfo",
                table: "Inputs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoreInfo",
                table: "Outputs");

            migrationBuilder.DropColumn(
                name: "MoreInfo",
                table: "Inputs");
        }
    }
}
