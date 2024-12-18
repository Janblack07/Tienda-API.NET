using API_TIENDA.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API Tienda",
        Version = "v1",
        Description = "Documentación para la API de la Tienda"
    });
});

// --- Configuración de JWT ---
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Secret"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// --- Servicios Personalizados ---
builder.Services.AddScoped<AuthService>();
builder.Services.AddDbContext<DbTienda>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar configuración de Cloudinary
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

var app = builder.Build();

// --- Migraciones y Seeders ---
// Este es el lugar donde colocas el bloque para asegurarte que las migraciones se apliquen y los datos iniciales estén presentes
//using (var scope = app.Services.CreateScope())
//{
  //  var context = scope.ServiceProvider.GetRequiredService<DbTienda>();
    //context.Database.EnsureDeleted();  // Borra la base de datos actual (si es necesario)
    //context.Database.EnsureCreated(); // Vuelve a crear la base de datos
    //context.Database.Migrate(); // Aplica las migraciones
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Tienda v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Esto debe ir antes de UseAuthorization
app.UseAuthorization();
app.MapControllers();

app.Run();
