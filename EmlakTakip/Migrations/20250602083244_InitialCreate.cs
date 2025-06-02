using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmlakTakip.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emlaklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IlanTipi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enlem = table.Column<double>(type: "float", nullable: true),
                    Boylam = table.Column<double>(type: "float", nullable: true),
                    Metrekare = table.Column<int>(type: "int", nullable: false),
                    OdaSayisi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsitmaTipi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BinaYasi = table.Column<int>(type: "int", nullable: false),
                    BulunduguKat = table.Column<int>(type: "int", nullable: false),
                    KatSayisi = table.Column<int>(type: "int", nullable: false),
                    EsyaliMi = table.Column<bool>(type: "bit", nullable: false),
                    BanyoSayisi = table.Column<int>(type: "int", nullable: false),
                    BalkonVarMi = table.Column<bool>(type: "bit", nullable: false),
                    SiteIciMi = table.Column<bool>(type: "bit", nullable: false),
                    KrediliMi = table.Column<bool>(type: "bit", nullable: false),
                    GorselYolu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IlanTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emlaklar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmlakFotolar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaYolu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmlakId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmlakFotolar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmlakFotolar_Emlaklar_EmlakId",
                        column: x => x.EmlakId,
                        principalTable: "Emlaklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmlakFotolar_EmlakId",
                table: "EmlakFotolar",
                column: "EmlakId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmlakFotolar");

            migrationBuilder.DropTable(
                name: "Emlaklar");
        }
    }
}
