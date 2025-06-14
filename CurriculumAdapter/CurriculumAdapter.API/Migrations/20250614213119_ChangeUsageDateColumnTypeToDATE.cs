using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurriculumAdapter.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUsageDateColumnTypeToDATE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "usage_date",
                table: "feature_usage_logs",
                type: "DATE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "usage_date",
                table: "feature_usage_logs",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATE");
        }
    }
}
