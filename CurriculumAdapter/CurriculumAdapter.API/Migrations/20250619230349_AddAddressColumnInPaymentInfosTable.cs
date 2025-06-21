using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurriculumAdapter.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressColumnInPaymentInfosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "payment_infos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "payment_infos");
        }
    }
}
