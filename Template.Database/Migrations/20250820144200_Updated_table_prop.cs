using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Database.Migrations
{
    /// <inheritdoc />
    public partial class Updatedtableprop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                schema: "comms",
                table: "TblEmailConfig",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                schema: "comms",
                table: "TblEmailQueue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CCEmailAddresses",
                schema: "comms",
                table: "TblEmailQueue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromEmailAddress",
                schema: "comms",
                table: "TblEmailQueue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                schema: "comms",
                table: "TblEmailQueue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SendAttempts",
                schema: "comms",
                table: "TblEmailQueue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "comms",
                table: "TblEmailQueue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                schema: "comms",
                table: "TblEmailQueue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToEmailAddresses",
                schema: "comms",
                table: "TblEmailQueue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "feba6c0a-e24c-4410-a8c2-0145bd3d1853",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0692593e-4d53-4d35-96ba-1c8d33e3e041", new DateTime(2025, 8, 20, 16, 41, 59, 933, DateTimeKind.Local).AddTicks(8335), "AQAAAAIAAYagAAAAEF/YxhrNbqSr7dDwHREDHXD9Xvg1v1Hj+uXdwspqyd1VkOITt1DuC14OPpCA8kHfFg==", "6083753f-8f92-422f-9605-c5d05fabdaaf" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                schema: "comms",
                table: "TblEmailQueue");

            migrationBuilder.DropColumn(
                name: "CCEmailAddresses",
                schema: "comms",
                table: "TblEmailQueue");

            migrationBuilder.DropColumn(
                name: "FromEmailAddress",
                schema: "comms",
                table: "TblEmailQueue");

            migrationBuilder.DropColumn(
                name: "Priority",
                schema: "comms",
                table: "TblEmailQueue");

            migrationBuilder.DropColumn(
                name: "SendAttempts",
                schema: "comms",
                table: "TblEmailQueue");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "comms",
                table: "TblEmailQueue");

            migrationBuilder.DropColumn(
                name: "Subject",
                schema: "comms",
                table: "TblEmailQueue");

            migrationBuilder.DropColumn(
                name: "ToEmailAddresses",
                schema: "comms",
                table: "TblEmailQueue");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "comms",
                table: "TblEmailConfig",
                newName: "Username");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "feba6c0a-e24c-4410-a8c2-0145bd3d1853",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "44c7a754-d2db-4a3d-80c3-1dc4d6b9088a", new DateTime(2025, 8, 20, 11, 36, 47, 615, DateTimeKind.Local).AddTicks(1610), "AQAAAAIAAYagAAAAEGhetnRqGG3utF4Kk36XgUl8JYVTfqvXgTmwlLjQ4LxrBPkFYhyByu6ElQs0zJuteA==", "524f484b-589a-4327-a8ba-1529da95aee7" });
        }
    }
}
