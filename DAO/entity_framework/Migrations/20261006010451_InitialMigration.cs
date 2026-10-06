using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

// MIGRACION INICIAL
// Este archivo lo genera automaticamente el comando:
//   dotnet ef migrations add InitialMigration --project DAO --startup-project API
// EF lee las entidades y el AppDbContext y escribe aca, en C#, las instrucciones
// para crear las tablas. Cuando corremos "dotnet ef database update", EF traduce
// esto a SQL (CREATE TABLE ...) y lo ejecuta en MySQL.
// El numero del nombre del archivo (20261006010451) es la fecha y hora de creacion:
// sirve para que las migraciones se apliquen siempre en orden.

namespace DAO.entity_framework.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        // Up(): lo que se ejecuta al APLICAR la migracion (database update).
        // Aca se crean todas las tablas.
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Configura la base para usar utf8mb4 (permite acentos, ñ y emojis).
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            // ---------------- TABLA Courses (entidad Course) ----------------
            migrationBuilder.CreateTable(
                name: "Courses", // Nombre de la tabla = nombre del DbSet en AppDbContext
                columns: table => new
                {
                    // Cada propiedad publica de la entidad se convierte en una columna.
                    // long en C# -> bigint en MySQL. nullable: false = obligatorio (NOT NULL).
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        // IdentityColumn = AUTO_INCREMENT: MySQL genera el Id solo (1, 2, 3...)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    // string en C# -> longtext en MySQL (texto sin limite de largo)
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    // Clave primaria (PK): la propiedad "Id" se detecta por convencion de nombre.
                    table.PrimaryKey("PK_Courses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // ---------------- TABLA Students (entidad Student) ----------------
            // Student hereda de Person, por eso tiene Name, Age y Dni ademas de File (legajo).
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    File = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Age = table.Column<int>(type: "int", nullable: false), // int en C# -> int en MySQL
                    Dni = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // ---------------- TABLA Teams (entidad Team) ----------------
            // La lista "Players" de Team NO es una columna: es una relacion,
            // y se guarda como TeamId en la tabla Players (ver mas abajo).
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // ---------------- TABLA Trainers (entidad Trainer) ----------------
            // Trainer no tiene propiedades propias: todas las columnas vienen de Person.
            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Dni = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // ---------------- TABLA Activities (entidad Activity) ----------------
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    // DateTime en C# -> datetime(6) en MySQL (fecha y hora con microsegundos)
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    // El enum TypeActivity se guarda como numero: Exam=0, Homework=1, PracticalWork=2, Quiz=3
                    Type = table.Column<int>(type: "int", nullable: false),
                    // CourseId no existe en la clase Activity: EF la crea sola porque
                    // Course tiene una List<Activity> (relacion uno a muchos: un curso tiene muchas actividades).
                    // nullable: true = una actividad puede no tener curso asignado.
                    CourseId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    // Clave foranea (FK): CourseId tiene que coincidir con un Id existente en Courses.
                    table.ForeignKey(
                        name: "FK_Activities_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // ---------------- TABLA CourseStudent (tabla intermedia) ----------------
            // Relacion muchos a muchos: un Student tiene List<Course> y un Course tiene List<Student>.
            // Una base de datos no puede guardar listas en una columna, entonces EF crea
            // esta tabla extra donde cada fila dice "el alumno X esta en el curso Y".
            migrationBuilder.CreateTable(
                name: "CourseStudent",
                columns: table => new
                {
                    CoursesId = table.Column<long>(type: "bigint", nullable: false),
                    StudentsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    // Clave primaria compuesta: la combinacion (curso, alumno) no se puede repetir.
                    table.PrimaryKey("PK_CourseStudent", x => new { x.CoursesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_CourseStudent_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        // Cascade: si se borra un curso, se borran automaticamente sus filas en esta tabla.
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        // Lo mismo si se borra un alumno.
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // ---------------- TABLA Players (entidad Player) ----------------
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    // TeamId si existe en la clase Player (long?). El "?" la hace opcional: nullable: true.
                    TeamId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Dni = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    // FK: relacion uno a muchos, un equipo tiene muchos jugadores.
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // ---------------- INDICES ----------------
            // Un indice acelera las busquedas por esa columna (como el indice de un libro).
            // EF crea uno en cada clave foranea, porque se usan mucho para unir tablas (JOIN).
            migrationBuilder.CreateIndex(
                name: "IX_Activities_CourseId",
                table: "Activities",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudent_StudentsId",
                table: "CourseStudent",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");
        }

        // Down(): lo contrario de Up(). Se ejecuta si deshacemos la migracion
        // (por ejemplo con "dotnet ef database update 0"). Borra las tablas.
        // El orden importa: primero las que tienen claves foraneas (Activities,
        // CourseStudent, Players) y despues las tablas a las que apuntan.
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "CourseStudent");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
