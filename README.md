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
