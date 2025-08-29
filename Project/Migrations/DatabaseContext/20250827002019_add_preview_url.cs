using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace image_upload_api.Migrations.DatabaseContext
{
    /// <inheritdoc />
    public partial class add_preview_url : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviewUrl",
                table: "Images",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviewUrl",
                table: "Images");
        }
    }
}
