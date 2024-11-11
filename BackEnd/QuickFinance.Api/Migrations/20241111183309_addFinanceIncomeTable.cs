using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class addFinanceIncomeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalExpenses",
                table: "FinanceEvaluations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalIncomes",
                table: "FinanceEvaluations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "FinanceIncomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinanceId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceIncomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinanceIncomes_FinanceEvaluations_FinanceId",
                        column: x => x.FinanceId,
                        principalTable: "FinanceEvaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinanceIncomes_FinanceId",
                table: "FinanceIncomes",
                column: "FinanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinanceIncomes");

            migrationBuilder.DropColumn(
                name: "TotalExpenses",
                table: "FinanceEvaluations");

            migrationBuilder.DropColumn(
                name: "TotalIncomes",
                table: "FinanceEvaluations");
        }
    }
}
