using ProductosAPI.Models;

namespace ProductosAPI.Repositories;

// La interface es el "contrato": dice QUE metodos debe tener el repositorio,
// pero no COMO se hacen. Esta es la que pidio el maestro.
public interface IProductoRepository
{
    List<Producto> ObtenerTodos();
    Producto? ObtenerPorId(int id);
    int Insertar(Producto producto);
    bool Actualizar(Producto producto);
    bool Eliminar(int id);
}
