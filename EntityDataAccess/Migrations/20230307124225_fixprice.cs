using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutputPrice",
                table: "InputInfos");

            migrationBuilder.AddColumn<double>(
                name: "OutputPrice",
                table: "OutputInfos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutputPrice",
                table: "OutputInfos");

            migrationBuilder.AddColumn<double>(
                name: "OutputPrice",
                table: "InputInfos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
