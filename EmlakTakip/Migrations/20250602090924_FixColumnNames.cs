using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmlakTakip.Migrations
{
    /// <inheritdoc />
    public partial class FixColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GorselYolu",
                table: "Emlaklar");

            migrationBuilder.RenameColumn(
                name: "IlanTipi",
                table: "Emlaklar",
                newName: "Tip");

            migrationBuilder.RenameColumn(
                name: "BalkonVarMi",
                table: "Emlaklar",
                newName: "Balkon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tip",
                table: "Emlaklar",
                newName: "IlanTipi");

            migrationBuilder.RenameColumn(
                name: "Balkon",
                table: "Emlaklar",
                newName: "BalkonVarMi");

            migrationBuilder.AddColumn<string>(
                name: "GorselYolu",
                table: "Emlaklar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
