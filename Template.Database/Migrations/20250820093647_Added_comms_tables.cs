using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Database.Migrations
{
    /// <inheritdoc />
    public partial class Addedcommstables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "comms");

            migrationBuilder.CreateTable(
                name: "TblEmailConfig",
                schema: "comms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Port = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblEmailConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblEmailQueue",
                schema: "comms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_TblEmailQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblEmailTemplat",
                schema: "comms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblEmailTemplat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblNotification",
                schema: "comms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_TblNotification", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "feba6c0a-e24c-4410-a8c2-0145bd3d1853",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "44c7a754-d2db-4a3d-80c3-1dc4d6b9088a", new DateTime(2025, 8, 20, 11, 36, 47, 615, DateTimeKind.Local).AddTicks(1610), "AQAAAAIAAYagAAAAEGhetnRqGG3utF4Kk36XgUl8JYVTfqvXgTmwlLjQ4LxrBPkFYhyByu6ElQs0zJuteA==", "524f484b-589a-4327-a8ba-1529da95aee7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblEmailConfig",
                schema: "comms");

            migrationBuilder.DropTable(
                name: "TblEmailQueue",
                schema: "comms");

            migrationBuilder.DropTable(
                name: "TblEmailTemplat",
                schema: "comms");

            migrationBuilder.DropTable(
                name: "TblNotification",
                schema: "comms");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "feba6c0a-e24c-4410-a8c2-0145bd3d1853",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "957fab7e-66af-416d-99ef-0c215fe19bc8", new DateTime(2025, 8, 17, 20, 47, 33, 559, DateTimeKind.Local).AddTicks(1076), "AQAAAAIAAYagAAAAELnWimmA8Q87o0azGzf/nGPfmU9KyjtyWDmU4IEXUEsl0ga7D2KC1ZNj/Pd0PWIJCg==", "ba73e727-e0af-4b93-a8bd-acc1fb0a6c71" });
        }
    }
}
