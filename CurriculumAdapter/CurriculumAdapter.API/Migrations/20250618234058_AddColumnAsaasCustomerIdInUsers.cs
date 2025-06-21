using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurriculumAdapter.API.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnAsaasCustomerIdInUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "asaas_customer_id",
                table: "users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "asaas_customer_id",
                table: "users");
        }
    }
}
