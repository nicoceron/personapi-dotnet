using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace personapi_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureAllCascadeDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "persona",
                columns: table => new
                {
                    cc = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    apellido = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    genero = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    edad = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__persona__3213E83FC333E091", x => x.cc);
                });

            migrationBuilder.CreateTable(
                name: "profesion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    nom = table.Column<string>(type: "varchar(90)", unicode: false, maxLength: 90, nullable: false),
                    des = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__profesio__3213E83F50854D8C", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "telefono",
                columns: table => new
                {
                    num = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    oper = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    duenio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__telefono__DF908D65F944ADC4", x => x.num);
                    table.ForeignKey(
                        name: "FK__telefono__duenio__3B75D760",
                        column: x => x.duenio,
                        principalTable: "persona",
                        principalColumn: "cc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "estudios",
                columns: table => new
                {
                    id_prof = table.Column<int>(type: "int", nullable: false),
                    cc_per = table.Column<int>(type: "int", nullable: false),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true),
                    univer = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__estudios__FB3F71A6223C4A76", x => new { x.id_prof, x.cc_per });
                    table.ForeignKey(
                        name: "FK__estudios__cc_per__3F466844",
                        column: x => x.cc_per,
                        principalTable: "persona",
                        principalColumn: "cc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__estudios__id_pro__3E52440B",
                        column: x => x.id_prof,
                        principalTable: "profesion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_estudios_cc_per",
                table: "estudios",
                column: "cc_per");

            migrationBuilder.CreateIndex(
                name: "IX_telefono_duenio",
                table: "telefono",
                column: "duenio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "estudios");

            migrationBuilder.DropTable(
                name: "telefono");

            migrationBuilder.DropTable(
                name: "profesion");

            migrationBuilder.DropTable(
                name: "persona");
        }
    }
}
