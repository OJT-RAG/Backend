using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OJT_RAG.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddSemesterCompanyIdAndRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "semester_company_id",
                table: "job_position",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_position_semester_company_id",
                table: "job_position",
                column: "semester_company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_job_position_semester_company_semester_company_id",
                table: "job_position",
                column: "semester_company_id",
                principalTable: "semester_company",
                principalColumn: "semester_company_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_job_position_semester_company_semester_company_id",
                table: "job_position");

            migrationBuilder.DropIndex(
                name: "IX_job_position_semester_company_id",
                table: "job_position");

            migrationBuilder.DropColumn(
                name: "semester_company_id",
                table: "job_position");
        }
    }
}
