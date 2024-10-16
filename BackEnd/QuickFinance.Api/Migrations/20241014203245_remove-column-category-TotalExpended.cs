using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Data;

#nullable disable

namespace QuickFinance.Api.Migrations
{
    /// <inheritdoc />
    public partial class removecolumncategoryTotalExpended : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //--Drop the default constraint on the TotalExpended column
            migrationBuilder.Sql(@" ALTER TABLE [dbo].[Categories] 
    DROP CONSTRAINT [DF__Categorie__Total__2A164134];");


            //-- Drop the TotalExpended column
            migrationBuilder.Sql(@"
    ALTER TABLE [dbo].[Categories] 
    DROP COLUMN [TotalExpended];");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
