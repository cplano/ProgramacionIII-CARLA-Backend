// Program.cs es el PUNTO DE ENTRADA de la API: es lo primero que se ejecuta.
// Tiene dos partes:
//   1) Registrar servicios (builder.Services...): que cosas va a poder usar la app.
//   2) Configurar el pipeline (app.Use...): que pasa con cada pedido HTTP que llega.

using DAO;                           // Para usar las clases DAO (PlayerDAO, TeamDAO, etc.)
using DAO.entity_framework;          // Para usar AppDbContext
using Microsoft.EntityFrameworkCore; // Para AddDbContext y UseMySql

// Crea el "constructor" de la aplicacion. Lee automaticamente appsettings.json
// y appsettings.Development.json (si estamos en desarrollo).
var builder = WebApplication.CreateBuilder(args);

// ======================= 1) REGISTRO DE SERVICIOS =======================

// Habilita los Controllers (las clases de la carpeta Controllers que responden a las URLs).
builder.Services.AddControllers();

// CONEXION A LA BASE DE DATOS (Entity Framework + MySQL)
// Lee la connection string "DefaultConnection" del appsettings.Development.json.
// El "!" le dice al compilador "confia, no va a ser null".
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
// Registra AppDbContext para que se pueda inyectar en otras clases.
// UseMySql indica que la base es MySQL (paquete Pomelo).
// ServerVersion.AutoDetect se conecta al servidor para averiguar su version (ej: 8.0.46).
builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// INYECCION DE DEPENDENCIAS DE LOS DAO
// Registramos cada DAO para que ASP.NET lo cree solo y se lo pase al Controller
// que lo pida en su constructor (no hace falta hacer "new PlayerDAO()").
// AddScoped = se crea UNA instancia nueva por cada pedido HTTP.
builder.Services.AddScoped<PlayerDAO>();
builder.Services.AddScoped<TeamDAO>();
builder.Services.AddScoped<StudentDAO>();
builder.Services.AddScoped<CourseDAO>();

// Swagger: genera automaticamente una pagina web (/swagger) para ver y probar
// todos los endpoints de la API sin necesidad de Postman.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Construye la aplicacion con todo lo registrado arriba.
var app = builder.Build();

// ======================= 2) PIPELINE HTTP =======================
// Cada pedido que llega pasa por estos pasos, en este orden.

// Swagger solo se activa en desarrollo (no en produccion).
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Si el pedido llega por http, lo redirige a https.
app.UseAuthorization();    // Verifica permisos (por ahora no hay ninguno configurado).
app.MapControllers();      // Conecta cada URL (ej: /api/player) con su Controller.

app.Run(); // Arranca el servidor y queda escuchando pedidos.
