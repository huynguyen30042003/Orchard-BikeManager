using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThuVienMedia.Migrations
{
    /// <inheritdoc />
    public partial class ThuVienMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CMThuVienMedia",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IDChuyenMucCapCha = table.Column<Guid>(type: "uniqueidentifier", maxLength: 40, nullable: true),
                    TenChuyenMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ThuTuSapXep = table.Column<byte>(type: "tinyint", nullable: true),
                    SuDung = table.Column<bool>(type: "bit", nullable: false),
                    ChuyenMucCapChaID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMThuVienMedia", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CMThuVienMedia_CMThuVienMedia_ChuyenMucCapChaID",
                        column: x => x.ChuyenMucCapChaID,
                        principalTable: "CMThuVienMedia",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ThuVienMedia",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IDChuyenMucMedia = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenThuVien = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GioiThieu = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UrlThumbAnhDaiDien = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UrlAnhDaiDien = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LoaiThuVien = table.Column<byte>(type: "tinyint", nullable: true),
                    IDFileMediaList = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    ThuTuSapXep = table.Column<byte>(type: "tinyint", nullable: true),
                    SuDung = table.Column<bool>(type: "bit", nullable: false),
                    ChuyenMucMediaID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuVienMedia", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ThuVienMedia_CMThuVienMedia_ChuyenMucMediaID",
                        column: x => x.ChuyenMucMediaID,
                        principalTable: "CMThuVienMedia",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FileMedia",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    LastModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IDThuVienMedia = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenFile = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TieuDe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    TacGia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdFile = table.Column<int>(type: "int", nullable: true),
                    UrlFile = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UrlThumb = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UrlAnhDaiDienVideo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LoaiFile = table.Column<byte>(type: "tinyint", nullable: true),
                    NguonFile = table.Column<byte>(type: "tinyint", nullable: true),
                    ThuTuSapXep = table.Column<byte>(type: "tinyint", nullable: true),
                    ThuVienMediaID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileMedia", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FileMedia_ThuVienMedia_IDThuVienMedia",
                        column: x => x.IDThuVienMedia,
                        principalTable: "ThuVienMedia",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileMedia_ThuVienMedia_ThuVienMediaID",
                        column: x => x.ThuVienMediaID,
                        principalTable: "ThuVienMedia",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CMThuVienMedia_ChuyenMucCapChaID",
                table: "CMThuVienMedia",
                column: "ChuyenMucCapChaID");

            migrationBuilder.CreateIndex(
                name: "IX_FileMedia_IDThuVienMedia",
                table: "FileMedia",
                column: "IDThuVienMedia");

            migrationBuilder.CreateIndex(
                name: "IX_FileMedia_ThuVienMediaID",
                table: "FileMedia",
                column: "ThuVienMediaID");

            migrationBuilder.CreateIndex(
                name: "IX_ThuVienMedia_ChuyenMucMediaID",
                table: "ThuVienMedia",
                column: "ChuyenMucMediaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileMedia");

            migrationBuilder.DropTable(
                name: "ThuVienMedia");

            migrationBuilder.DropTable(
                name: "CMThuVienMedia");
        }
    }
}
