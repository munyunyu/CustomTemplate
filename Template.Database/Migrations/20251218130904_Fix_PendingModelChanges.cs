using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Database.Migrations
{
    /// <inheritdoc />
    public partial class Fix_PendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "feba6c0a-e24c-4410-a8c2-0145bd3d1853",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBYiAwJWV5Ovgxv08Ov+gqyH3jZgvoWhlSiiA9HY6EzEPH9sA5NcaZuxnu/0LcGiKg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "feba6c0a-e24c-4410-a8c2-0145bd3d1853",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELvzldaMcm/Dauu+1bUVGtCFEoogD1b7LTBv0Y8AxI+tcLeCc8J8mN+WkNgu+WJzPg==");
        }
    }
}
