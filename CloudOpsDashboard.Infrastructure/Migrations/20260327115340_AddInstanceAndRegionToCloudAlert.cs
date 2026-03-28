using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudOpsDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInstanceAndRegionToCloudAlert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudAlerts_CloudInstances_CloudInstanceId",
                table: "CloudAlerts");

            migrationBuilder.DropIndex(
                name: "IX_CloudAlerts_CloudInstanceId",
                table: "CloudAlerts");

            migrationBuilder.DropColumn(
                name: "CloudInstanceId",
                table: "CloudAlerts");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "CloudAlerts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Severity",
                table: "CloudAlerts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CloudAlerts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Instance",
                table: "CloudAlerts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "CloudAlerts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instance",
                table: "CloudAlerts");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "CloudAlerts");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "CloudAlerts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Severity",
                table: "CloudAlerts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CloudAlerts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "CloudInstanceId",
                table: "CloudAlerts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CloudAlerts_CloudInstanceId",
                table: "CloudAlerts",
                column: "CloudInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudAlerts_CloudInstances_CloudInstanceId",
                table: "CloudAlerts",
                column: "CloudInstanceId",
                principalTable: "CloudInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
