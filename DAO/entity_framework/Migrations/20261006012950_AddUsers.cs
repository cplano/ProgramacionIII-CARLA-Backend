// SEGUNDA MIGRACION: agrega la tabla Users para el login.
// Se genero con: dotnet ef migrations add AddUsers --project DAO --startup-project API --output-dir entity_framework/Migrations
// EF comparo las entidades con AppDbContextModelSnapshot.cs, vio que se agrego
// DbSet<User> y escribio SOLO esa diferencia (las otras tablas ya existian).
// Se aplico con: dotnet ef database update --project DAO --startup-project API

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.entity_framework.Migrations
{
    /// <inheritdoc />
    public partial class AddUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    // [StringLength(100)] en la entidad -> varchar(100) (texto con largo maximo) en vez de longtext
                    Lastname = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    // Se guarda el HASH de BCrypt, nunca la contraseña real
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Dni = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Indice UNICO sobre Email (configurado en OnModelCreating del AppDbContext):
            // MySQL rechaza cualquier INSERT con un email que ya exista.
            // Por eso Email necesita largo maximo: MySQL no permite indices sobre longtext.
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        // Down: deshace la migracion borrando la tabla Users.
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
