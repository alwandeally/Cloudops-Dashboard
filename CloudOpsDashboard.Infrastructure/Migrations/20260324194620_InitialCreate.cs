using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudOpsDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CloudInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstanceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    CpuUsage = table.Column<double>(type: "float", nullable: false),
                    MemoryUsage = table.Column<double>(type: "float", nullable: false),
                    UptimePercentage = table.Column<double>(type: "float", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CloudInstances_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CloudAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CloudInstanceId = table.Column<int>(type: "int", nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CloudAlerts_CloudInstances_CloudInstanceId",
                        column: x => x.CloudInstanceId,
                        principalTable: "CloudInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstanceMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CloudInstanceId = table.Column<int>(type: "int", nullable: false),
                    CpuUsage = table.Column<double>(type: "float", nullable: false),
                    MemoryUsage = table.Column<double>(type: "float", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstanceMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstanceMetrics_CloudInstances_CloudInstanceId",
                        column: x => x.CloudInstanceId,
                        principalTable: "CloudInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloudAlerts_CloudInstanceId",
                table: "CloudAlerts",
                column: "CloudInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CloudInstances_RegionId",
                table: "CloudInstances",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_InstanceMetrics_CloudInstanceId",
                table: "InstanceMetrics",
                column: "CloudInstanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CloudAlerts");

            migrationBuilder.DropTable(
                name: "InstanceMetrics");

            migrationBuilder.DropTable(
                name: "CloudInstances");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
