// Program.cs es el PUNTO DE ENTRADA de la API: es lo primero que se ejecuta.
// Tiene dos partes:
//   1) Registrar servicios (builder.Services...): que cosas va a poder usar la app.
//   2) Configurar el pipeline (app.Use...): que pasa con cada pedido HTTP que llega.

using System.Text;
using API.Services;                                // TokenService (genera los JWT)
using DAO;                                         // Para usar las clases DAO (PlayerDAO, TeamDAO, etc.)
using DAO.entity_framework;                        // Para usar AppDbContext
using Microsoft.AspNetCore.Authentication.JwtBearer; // Autenticacion con tokens JWT
using Microsoft.EntityFrameworkCore;               // Para AddDbContext y UseMySql
using Microsoft.IdentityModel.Tokens;              // Para validar la firma del token
using Microsoft.OpenApi.Models;                    // Para configurar el boton "Authorize" de Swagger

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
builder.Services.AddScoped<UserDAO>();

// Servicio que genera los tokens JWT en el login.
builder.Services.AddScoped<TokenService>();

// AUTENTICACION CON JWT
// Le dice a la API como verificar los tokens que llegan en el encabezado
// "Authorization: Bearer <token>" de los endpoints con [Authorize].
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,           // Que lo haya emitido nuestra API
            ValidateAudience = true,         // Que sea para nuestro frontend
            ValidateLifetime = true,         // Que no este vencido
            ValidateIssuerSigningKey = true, // Que la FIRMA sea valida (no fue modificado)
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            // La misma clave secreta que se usa para firmar en TokenService
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// CORS (Cross-Origin Resource Sharing)
// El navegador BLOQUEA por seguridad que una pagina (Angular en localhost:4200)
// haga pedidos a otro "origen" (la API en localhost:5006), salvo que la API lo permita.
// Esta politica permite pedidos desde la URL del frontend.
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins(builder.Configuration["FrontendUrl"] ?? "http://localhost:4200")
              .AllowAnyHeader()   // Permite cualquier encabezado (Content-Type, Authorization...)
              .AllowAnyMethod()); // Permite GET, POST, PUT, DELETE
});

// Swagger: genera automaticamente una pagina web (/swagger) para ver y probar
// todos los endpoints de la API sin necesidad de Postman.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Agrega el boton "Authorize" en Swagger para pegar el token y probar
    // los endpoints protegidos con [Authorize].
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pegar solo el token (sin la palabra Bearer)"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

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
app.UseCors("Frontend");   // Aplica la politica CORS (antes de autenticar)
app.UseAuthentication();   // 1ro: lee el token y averigua QUIEN es el usuario
app.UseAuthorization();    // 2do: decide si ese usuario PUEDE usar el endpoint ([Authorize])
app.MapControllers();      // Conecta cada URL (ej: /api/player) con su Controller.

app.Run(); // Arranca el servidor y queda escuchando pedidos.
