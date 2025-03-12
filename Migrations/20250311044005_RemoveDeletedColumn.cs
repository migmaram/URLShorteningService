using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLShorteningService.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDeletedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Urls");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Urls",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
