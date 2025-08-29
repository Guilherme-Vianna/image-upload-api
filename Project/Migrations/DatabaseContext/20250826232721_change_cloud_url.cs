using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace image_upload_api.Migrations.DatabaseContext
{
    /// <inheritdoc />
    public partial class change_cloud_url : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CloudnaryLink",
                table: "Images",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CloudnaryLink",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
