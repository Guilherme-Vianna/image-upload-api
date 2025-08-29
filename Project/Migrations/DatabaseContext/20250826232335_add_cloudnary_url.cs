using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace image_upload_api.Migrations.DatabaseContext
{
    /// <inheritdoc />
    public partial class add_cloudnary_url : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CloudnaryLink",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloudnaryLink",
                table: "Images");
        }
    }
}
