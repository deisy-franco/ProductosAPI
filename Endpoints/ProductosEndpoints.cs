using ProductosAPI.Models;
using ProductosAPI.Repositories;

namespace ProductosAPI.Endpoints;

// Aqui definimos las rutas (URLs) de la API.
// Cada ruta recibe la peticion y llama al repositorio.
public static class ProductosEndpoints
{
    public static void Map(WebApplication app)
    {
        var grupo = app.MapGroup("/productos").WithTags("Productos");

        // GET /productos -> todos
        grupo.MapGet("/", (IProductoRepository repo) =>
            Results.Ok(repo.ObtenerTodos())
        );

        // GET /productos/5 -> buscar uno
        grupo.MapGet("/{id}", (int id, IProductoRepository repo) =>
        {
            var producto = repo.ObtenerPorId(id);
            return producto is null
                ? Results.NotFound(new { mensaje = "Producto no encontrado" })
                : Results.Ok(producto);
        });

        // POST /productos -> crear
        grupo.MapPost("/", (Producto nuevo, IProductoRepository repo) =>
        {
            int idNuevo = repo.Insertar(nuevo);
            if (idNuevo == 0)
                return Results.BadRequest(new { mensaje = "No se pudo guardar el producto" });

            nuevo.Id = idNuevo;
            return Results.Created($"/productos/{idNuevo}", nuevo); // 201
        });

        // PUT /productos/5 -> actualizar
        grupo.MapPut("/{id}", (int id, Producto producto, IProductoRepository repo) =>
        {
            producto.Id = id; // usamos el id de la URL
            return repo.Actualizar(producto)
                ? Results.NoContent() // 204
                : Results.NotFound(new { mensaje = "Producto no encontrado" });
        });

        // DELETE /productos/5 -> eliminar
        grupo.MapDelete("/{id}", (int id, IProductoRepository repo) =>
            repo.Eliminar(id)
                ? Results.NoContent() // 204
                : Results.NotFound(new { mensaje = "Producto no encontrado" })
        );
    }
}
