# Programación III - Backend (Carla Plano)

API REST en .NET 8 con arquitectura por capas.

| Proyecto   | Contenido                                                        |
|------------|------------------------------------------------------------------|
| `Entities` | Entidades del dominio (Person, Player, Trainer, Team, Student, Course, Activity) |
| `DAO`      | Acceso a datos (DAOs + `entity_framework/AppDbContext.cs`)       |
| `API`      | Controllers, configuración e inyección de dependencias           |

El frontend de la materia está en un repositorio aparte.

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- MySQL Server 8 (y MySQL Workbench para ver las tablas)
- La herramienta de migraciones: `dotnet tool install --global dotnet-ef`

## Estructura

```
CapasApp.sln
├── Entities/                 → clases del dominio (no referencia a nadie)
├── DAO/                      → acceso a datos (referencia a Entities)
│   ├── MockDatabase.cs       → listas en memoria para los ejercicios 1 y 2
│   ├── UserDAO.cs            → usuarios en MySQL con Entity Framework
│   └── entity_framework/     → AppDbContext y migraciones
└── API/                      → controllers, DTOs, JWT (referencia a DAO y Entities)
```

Regla de las capas: `API → DAO → Entities`. Las capas de abajo nunca conocen a las de arriba.

## Puertos

| Qué | URL |
|---|---|
| API (perfil http) | `http://localhost:5006` |
| Swagger | `http://localhost:5006/swagger` |
| API (perfil https) | `https://localhost:7250` |
| Frontend Angular (permitido por CORS) | `http://localhost:4200` |
| MySQL | `localhost:3306`, base `capasapp` |

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
3. Las migraciones ya están en el repo (`DAO/entity_framework/Migrations`: `InitialMigration`, `AddUsers` y
   `AddTrainerTeamAndOptionalDescription`). Al clonar alcanza con aplicarlas desde la carpeta raíz
   (donde está `CapasApp.sln`):

```bash
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

Se abre Swagger en `http://localhost:5006/swagger`. Los datos de Player, Team, Student, etc. viven en
`MockDatabase` y se pierden al reiniciar la API; los usuarios quedan guardados en MySQL.

## Ejercicios 1 y 2: Entities, DAO y API

Cada entidad de los ejercicios tiene su DAO (CRUD sobre `MockDatabase`) y su Controller:

| Ejercicio | Entidad | DAO | Endpoints |
|---|---|---|---|
| 1 | Player | `PlayerDAO` | `/api/Player` |
| 1 | Trainer | `TrainerDAO` | `/api/Trainer` |
| 1 | Team (tiene `Players` y `Trainers`) | `TeamDAO` | `/api/Team` |
| 2 | Student | `StudentDAO` | `/api/Student` |
| 2 | Course | `CourseDAO` | `/api/Course` |
| 2 | Activity | `ActivityDAO` | `/api/Activity` |

Todos los controllers exponen:

| Método | Ruta | Respuestas |
|---|---|---|
| `GET` | `/api/{Entidad}` | 200 |
| `GET` | `/api/{Entidad}/{id}` | 200, 404 |
| `POST` | `/api/{Entidad}` | 201, 400 |
| `PUT` | `/api/{Entidad}/{id}` | 200, 404 |
| `DELETE` | `/api/{Entidad}/{id}` | 204, 404 |

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

### Ejemplo de login

```http
POST http://localhost:5006/api/User/login
Content-Type: application/json

{ "email": "usuario@email.com", "password": "MiClave123" }
```

Respuesta `200 OK`:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "user": { "id": 1, "fullname": "Nombre Apellido", "dni": "...", "email": "usuario@email.com", "username": "..." }
}
```

Con email o contraseña incorrectos responde `401` con el mismo mensaje en los dos casos, para no dar pistas.
Para los endpoints con `[Authorize]`, el token se manda en el encabezado `Authorization: Bearer <token>`
y vence a los 60 minutos.

El recorrido completo de los datos (Angular → API → MySQL → Angular) está explicado en
el repo de frontend: `angular/primer-proyecto/RECORRIDO-LOGIN.md`.
