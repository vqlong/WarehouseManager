using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "OutputInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValueSql: "'OK'",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "InputInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValueSql: "'OK'",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "OutputInfos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValueSql: "'OK'");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "InputInfos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValueSql: "'OK'");
        }
    }
}
