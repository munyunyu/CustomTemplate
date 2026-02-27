using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemUsageStatsAndNotificationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                schema: "comms",
                table: "TblNotification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "comms",
                table: "TblNotification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "comms",
                table: "TblNotification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "comms",
                table: "TblNotification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TblSystemUsageStats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SnapshotDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalUsers = table.Column<int>(type: "int", nullable: false),
                    ConfirmedUsers = table.Column<int>(type: "int", nullable: false),
                    LockedUsers = table.Column<int>(type: "int", nullable: false),
                    PendingEmails = table.Column<int>(type: "int", nullable: false),
                    FailedEmails24h = table.Column<int>(type: "int", nullable: false),
                    JobsRun24h = table.Column<int>(type: "int", nullable: false),
                    FailedJobs24h = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblSystemUsageStats", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblSystemUsageStats");

            migrationBuilder.DropColumn(
                name: "Message",
                schema: "comms",
                table: "TblNotification");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "comms",
                table: "TblNotification");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "comms",
                table: "TblNotification");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "comms",
                table: "TblNotification");
        }
    }
}
