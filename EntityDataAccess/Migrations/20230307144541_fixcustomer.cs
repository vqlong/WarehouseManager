using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixcustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutputInfos_Customers_CustomerId",
                table: "OutputInfos");

            migrationBuilder.DropIndex(
                name: "IX_OutputInfos_CustomerId",
                table: "OutputInfos");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "OutputInfos");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Outputs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Outputs_CustomerId",
                table: "Outputs",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Outputs_Customers_CustomerId",
                table: "Outputs",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outputs_Customers_CustomerId",
                table: "Outputs");

            migrationBuilder.DropIndex(
                name: "IX_Outputs_CustomerId",
                table: "Outputs");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Outputs");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "OutputInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OutputInfos_CustomerId",
                table: "OutputInfos",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInfos_Customers_CustomerId",
                table: "OutputInfos",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
