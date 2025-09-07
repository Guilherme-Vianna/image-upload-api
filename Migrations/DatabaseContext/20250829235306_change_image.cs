using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace image_upload_api.Migrations.DatabaseContext
{
    /// <inheritdoc />
    public partial class change_image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageProcessingJobs");

            migrationBuilder.DropColumn(
                name: "PreviewUrl",
                table: "Images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviewUrl",
                table: "Images",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ImageProcessingJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageProcessingJobs", x => x.Id);
                });
        }
    }
}
