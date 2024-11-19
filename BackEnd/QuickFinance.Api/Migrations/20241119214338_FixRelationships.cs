using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_FinanceDetails_FinanceEvaluations_FinanceEvaluationId1",
            //    table: "FinanceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_FinanceIncomes_FinanceEvaluations_FinanceId",
                table: "FinanceIncomes");

            //migrationBuilder.DropIndex(
            //    name: "IX_FinanceDetails_FinanceEvaluationId1",
            //    table: "FinanceDetails");

            //migrationBuilder.DropColumn(
            //    name: "FinanceEvaluationId1",
            //    table: "FinanceDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_FinanceIncomes_FinanceEvaluations_FinanceId",
                table: "FinanceIncomes",
                column: "FinanceId",
                principalTable: "FinanceEvaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinanceIncomes_FinanceEvaluations_FinanceId",
                table: "FinanceIncomes");

            //migrationBuilder.AddColumn<int>(
            //    name: "FinanceEvaluationId1",
            //    table: "FinanceDetails",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_FinanceDetails_FinanceEvaluationId1",
            //    table: "FinanceDetails",
            //    column: "FinanceEvaluationId1");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FinanceDetails_FinanceEvaluations_FinanceEvaluationId1",
            //    table: "FinanceDetails",
            //    column: "FinanceEvaluationId1",
            //    principalTable: "FinanceEvaluations",
            //    principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinanceIncomes_FinanceEvaluations_FinanceId",
                table: "FinanceIncomes",
                column: "FinanceId",
                principalTable: "FinanceEvaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
