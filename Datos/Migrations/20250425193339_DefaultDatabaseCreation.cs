using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datos.Migrations
{
    /// <inheritdoc />
    public partial class DefaultDatabaseCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreasConocimiento",
                columns: table => new
                {
                    IdAreaConocimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaveAreaConocimiento = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DescripcionAreaConocimiento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreasConocimiento", x => x.IdAreaConocimiento);
                });

            migrationBuilder.CreateTable(
                name: "Carreras",
                columns: table => new
                {
                    IdCarrera = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaveCarrera = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    NombreCarrera = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AliasCarrera = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EstadoCarrera = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreras", x => x.IdCarrera);
                });

            migrationBuilder.CreateTable(
                name: "Etapas",
                columns: table => new
                {
                    IdEtapa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaveEtapa = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    NombreEtapa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etapas", x => x.IdEtapa);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    IdMateria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaveMateria = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    NombreMateria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HC = table.Column<int>(type: "int", nullable: false),
                    HL = table.Column<int>(type: "int", nullable: false),
                    HT = table.Column<int>(type: "int", nullable: false),
                    HPC = table.Column<int>(type: "int", nullable: false),
                    HCL = table.Column<int>(type: "int", nullable: false),
                    HE = table.Column<int>(type: "int", nullable: false),
                    CR = table.Column<int>(type: "int", nullable: false),
                    PropositoGeneral = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Competencia = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Evidencia = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Metodologia = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Criterios = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    BibliografiaBasica = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    BibliografiaComplementaria = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    PerfilDocente = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    PathPUA = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EstadoMateria = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.IdMateria);
                });

            migrationBuilder.CreateTable(
                name: "PlanEstudios",
                columns: table => new
                {
                    IdPlanEstudio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanEstudio = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalCreditos = table.Column<int>(type: "int", nullable: false),
                    PerfilDeIngreso = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    PerfilDeEgreso = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    CampoOcupacional = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    EstadoPlanEstudio = table.Column<bool>(type: "bit", nullable: false),
                    IdCarrera = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanEstudios", x => x.IdPlanEstudio);
                    table.ForeignKey(
                        name: "FK_PlanEstudios_Carreras_IdCarrera",
                        column: x => x.IdCarrera,
                        principalTable: "Carreras",
                        principalColumn: "IdCarrera",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanEstudioMaterias",
                columns: table => new
                {
                    IdPlanEstudioMateria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Semestre = table.Column<int>(type: "int", nullable: false),
                    IdPlanEstudio = table.Column<int>(type: "int", nullable: false),
                    IdMateria = table.Column<int>(type: "int", nullable: false),
                    IdEtapa = table.Column<int>(type: "int", nullable: false),
                    IdAreaConocimiento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanEstudioMaterias", x => x.IdPlanEstudioMateria);
                    table.ForeignKey(
                        name: "FK_PlanEstudioMaterias_AreasConocimiento_IdAreaConocimiento",
                        column: x => x.IdAreaConocimiento,
                        principalTable: "AreasConocimiento",
                        principalColumn: "IdAreaConocimiento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanEstudioMaterias_Etapas_IdEtapa",
                        column: x => x.IdEtapa,
                        principalTable: "Etapas",
                        principalColumn: "IdEtapa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanEstudioMaterias_Materias_IdMateria",
                        column: x => x.IdMateria,
                        principalTable: "Materias",
                        principalColumn: "IdMateria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanEstudioMaterias_PlanEstudios_IdPlanEstudio",
                        column: x => x.IdPlanEstudio,
                        principalTable: "PlanEstudios",
                        principalColumn: "IdPlanEstudio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UK_ClaveAreaConocimiento",
                table: "AreasConocimiento",
                column: "ClaveAreaConocimiento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_AliasCarrera",
                table: "Carreras",
                column: "AliasCarrera",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_ClaveCarrera",
                table: "Carreras",
                column: "ClaveCarrera",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_NombreCarrera",
                table: "Carreras",
                column: "NombreCarrera",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_ClaveEtapa",
                table: "Etapas",
                column: "ClaveEtapa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_NombreEtapa",
                table: "Etapas",
                column: "NombreEtapa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_ClaveCarrera",
                table: "Materias",
                column: "ClaveMateria",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanEstudioMaterias_IdAreaConocimiento",
                table: "PlanEstudioMaterias",
                column: "IdAreaConocimiento");

            migrationBuilder.CreateIndex(
                name: "IX_PlanEstudioMaterias_IdEtapa",
                table: "PlanEstudioMaterias",
                column: "IdEtapa");

            migrationBuilder.CreateIndex(
                name: "IX_PlanEstudioMaterias_IdMateria",
                table: "PlanEstudioMaterias",
                column: "IdMateria");

            migrationBuilder.CreateIndex(
                name: "IX_PlanEstudioMaterias_IdPlanEstudio",
                table: "PlanEstudioMaterias",
                column: "IdPlanEstudio");

            migrationBuilder.CreateIndex(
                name: "IX_PlanEstudios_IdCarrera",
                table: "PlanEstudios",
                column: "IdCarrera");

            migrationBuilder.CreateIndex(
                name: "UK_PlaEstudio",
                table: "PlanEstudios",
                column: "PlanEstudio",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanEstudioMaterias");

            migrationBuilder.DropTable(
                name: "AreasConocimiento");

            migrationBuilder.DropTable(
                name: "Etapas");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "PlanEstudios");

            migrationBuilder.DropTable(
                name: "Carreras");
        }
    }
}
