using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurriculumAdapter.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentInfosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payment_infos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    postal_code = table.Column<string>(type: "text", nullable: false),
                    adress_number = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    cpf_cnpj = table.Column<string>(type: "text", nullable: false),
                    credit_card_token = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_infos", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payment_infos");
        }
    }
}
