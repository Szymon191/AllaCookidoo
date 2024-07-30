using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllaCookidoo.Migrations
{
    /// <inheritdoc />
    public partial class modifyfeedbackentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Evaluation",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Evaluation",
                table: "Feedbacks");
        }
    }
}
