using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirdRecogniser02.Data.Migrations
{
    /// <inheritdoc />
    public partial class userID_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Submission",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Submission",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Submission");
        }
    }
}
