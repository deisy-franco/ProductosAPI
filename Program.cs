using ProductosAPI.Data;
using ProductosAPI.Endpoints;
using ProductosAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Swagger: pagina para probar la API desde el navegador
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registramos nuestras clases para poder usarlas en los endpoints
builder.Services.AddSingleton<Conexion>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // se abre en http://localhost:5120/swagger
}

// Rutas de productos
ProductosEndpoints.Map(app);

app.Run();
