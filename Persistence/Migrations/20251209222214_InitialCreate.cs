using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogApplication",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    logDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogApplication", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    idusers = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameuser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passworduser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.idusers);
                });

            migrationBuilder.CreateTable(
                name: "taskuser",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idusers = table.Column<int>(type: "int", nullable: false),
                    nametask = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datetask = table.Column<DateTime>(type: "datetime2", nullable: false),
                    complete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taskuser", x => x.id);
                    table.ForeignKey(
                        name: "fk_taus_user",
                        column: x => x.idusers,
                        principalTable: "users",
                        principalColumn: "idusers");
                });

            migrationBuilder.CreateIndex(
                name: "IX_taskuser_idusers",
                table: "taskuser",
                column: "idusers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogApplication");

            migrationBuilder.DropTable(
                name: "taskuser");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
