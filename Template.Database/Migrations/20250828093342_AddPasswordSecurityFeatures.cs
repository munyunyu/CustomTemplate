using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordSecurityFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsManuallyLocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPasswordChangeDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutEndDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordExpiryDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PasswordNeverExpires",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TblPasswordHistory",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_TblPasswordHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblPasswordHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "feba6c0a-e24c-4410-a8c2-0145bd3d1853",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "FailedLoginAttempts", "IsManuallyLocked", "LastPasswordChangeDate", "LockoutEndDate", "PasswordExpiryDate", "PasswordHash", "PasswordNeverExpires", "SecurityStamp" },
                values: new object[] { "7601a664-a452-42b2-b56c-bd0d470619db", new DateTime(2025, 8, 28, 11, 33, 42, 231, DateTimeKind.Local).AddTicks(9905), 0, false, null, null, null, "AQAAAAIAAYagAAAAEKi/2n4hUb4wYUswExfLvZs/jr6fzMoI1Vr6iO0gwhVeO7gcq9a7TPgC3Vr2emskhQ==", false, "10dacb55-1477-4bac-a1ce-a0ee28e7529d" });

            migrationBuilder.UpdateData(
                schema: "comms",
                table: "TblEmailConfig",
                keyColumn: "Id",
                keyValue: new Guid("e7b52cbe-f96a-471d-9b8e-e5fd5c9f3c13"),
                columns: new[] { "Name", "SmtpPassword", "SmtpUser" },
                values: new object[] { "Default", "password", "user@example.com" });

            migrationBuilder.CreateIndex(
                name: "IX_TblPasswordHistory_UserId",
                schema: "user",
                table: "TblPasswordHistory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblPasswordHistory",
                schema: "user");

            migrationBuilder.DropColumn(
                name: "FailedLoginAttempts",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsManuallyLocked",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastPasswordChangeDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEndDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordExpiryDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordNeverExpires",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "feba6c0a-e24c-4410-a8c2-0145bd3d1853",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d5831419-3edc-4f50-87cf-449e084dcda4", new DateTime(2025, 8, 22, 12, 40, 23, 565, DateTimeKind.Local).AddTicks(7166), "AQAAAAIAAYagAAAAEHUPiul+cDCGt1ICETO6UIVmskSv47ft+Q6uII79KM5MZeMB4phTi+IE6a+EGvDtkA==", "8da370a5-7f7c-40c1-96e9-3094e90c56ae" });

            migrationBuilder.UpdateData(
                schema: "comms",
                table: "TblEmailConfig",
                keyColumn: "Id",
                keyValue: new Guid("e7b52cbe-f96a-471d-9b8e-e5fd5c9f3c13"),
                columns: new[] { "Name", "SmtpPassword", "SmtpUser" },
                values: new object[] { null, "tc#Prog219!", "percy.munyunyu@gmail.com" });
        }
    }
}
