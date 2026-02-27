using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Template.Database.Migrations
{
    /// <inheritdoc />
    public partial class SeedJobSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TblJobSchedule",
                columns: new[] { "Id", "CreatedById", "CreatedDate", "CronExpression", "Description", "Hash", "IsDeleted", "IsEnabled", "JobName", "LastRunStatus", "LastRunTime", "LastUpdatedById", "LastUpdatedDate", "NextRunTime" },
                values: new object[,]
                {
                    { new Guid("b1a1c2d3-e4f5-6789-abcd-ef0123456701"), new Guid("feba6c0a-e24c-4410-a8c2-0145bd3d1853"), new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "*/5 * * * *", null, null, false, true, "EmailServiceJob", 5, null, null, new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("b1a1c2d3-e4f5-6789-abcd-ef0123456702"), new Guid("feba6c0a-e24c-4410-a8c2-0145bd3d1853"), new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "0 0 * * *", null, null, false, true, "DataIntegrityJob", 5, null, null, new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("b1a1c2d3-e4f5-6789-abcd-ef0123456703"), new Guid("feba6c0a-e24c-4410-a8c2-0145bd3d1853"), new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "*/5 * * * *", null, null, false, true, "NotificationJob", 5, null, null, new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("b1a1c2d3-e4f5-6789-abcd-ef0123456704"), new Guid("feba6c0a-e24c-4410-a8c2-0145bd3d1853"), new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "0 23 * * *", null, null, false, true, "SystemUsageStatsJob", 5, null, null, new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TblJobSchedule",
                keyColumn: "Id",
                keyValue: new Guid("b1a1c2d3-e4f5-6789-abcd-ef0123456701"));

            migrationBuilder.DeleteData(
                table: "TblJobSchedule",
                keyColumn: "Id",
                keyValue: new Guid("b1a1c2d3-e4f5-6789-abcd-ef0123456702"));

            migrationBuilder.DeleteData(
                table: "TblJobSchedule",
                keyColumn: "Id",
                keyValue: new Guid("b1a1c2d3-e4f5-6789-abcd-ef0123456703"));

            migrationBuilder.DeleteData(
                table: "TblJobSchedule",
                keyColumn: "Id",
                keyValue: new Guid("b1a1c2d3-e4f5-6789-abcd-ef0123456704"));
        }
    }
}
