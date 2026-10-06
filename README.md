# Programación III - Backend (Carla Plano)

API REST en .NET 8 con arquitectura por capas.

| Proyecto   | Contenido                                                        |
|------------|------------------------------------------------------------------|
| `Entities` | Entidades del dominio (Person, Player, Trainer, Team, Student, Course, Activity) |
| `DAO`      | Acceso a datos (DAOs + `entity_framework/AppDbContext.cs`)       |
| `API`      | Controllers, configuración e inyección de dependencias           |

El frontend de la materia está en un repositorio aparte.

## Entity Framework + MySQL

Paquetes instalados en `DAO` y `API`:

- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Pomelo.EntityFrameworkCore.MySql

La connection string está en `API/appsettings.Development.json` (`DefaultConnection`).
`API/appsettings.json` (producción) está en el `.gitignore`.

### Crear la base de datos

1. Instalar MySQL Server y MySQL Workbench.
2. Crear el esquema `capasapp` y ajustar usuario/contraseña en `API/appsettings.Development.json`.
3. Desde la carpeta raíz (donde está `CapasApp.sln`):

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialMigration --project DAO --startup-project API --output-dir entity_framework/Migrations
dotnet ef database update --project DAO --startup-project API
```

Si cambia alguna entidad:

```bash
dotnet ef migrations add NombreDelCambio --project DAO --startup-project API --output-dir entity_framework/Migrations
dotnet ef database update --project DAO --startup-project API
```

## Ejecutar

```bash
dotnet run --project API
```

Swagger queda disponible en `/swagger`.

## Login (BCrypt + DTOs + JWT)

| Endpoint | Descripción | Respuestas |
|---|---|---|
| `POST /api/User/register` | Crea un usuario (la contraseña se guarda hasheada con BCrypt) | 201, 400, 409 (email repetido) |
| `POST /api/User/login` | Recibe `{ email, password }` y devuelve `{ token, user }` | 200, 400, 401 |
| `GET /api/User` | Lista de usuarios (requiere token) | 200, 401 |
| `GET /api/User/{id}` | Un usuario (requiere token) | 200, 401, 404 |

Archivos principales:

- `Entities/User.cs`: entidad con atributos de validación.
- `DAO/UserDAO.cs`: primer DAO que usa Entity Framework (MySQL real).
- `API/DTOs/`: `LoginDTO`, `RegisterDTO`, `UserResponseDTO`, `LoginResponseDTO`.
- `API/Helpers/PasswordHasher.cs`: BCrypt (`HashPassword` / `VerifyPassword`).
- `API/Services/TokenService.cs`: genera el JWT firmado.
- `API/Controllers/UserController.cs`: registro, login y endpoints protegidos con `[Authorize]`.
- `API/Program.cs`: configuración de JWT, CORS (para Angular en `localhost:4200`) y Swagger con botón **Authorize**.

Para probar los endpoints protegidos en Swagger: hacer login, copiar el `token`,
tocar **Authorize** y pegarlo.

El recorrido completo de los datos (Angular → API → MySQL → Angular) está explicado en
el repo de frontend: `angular/primer-proyecto/RECORRIDO-LOGIN.md`.
