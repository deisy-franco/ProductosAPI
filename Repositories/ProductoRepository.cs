using Microsoft.Data.SqlClient;
using ProductosAPI.Data;
using ProductosAPI.Models;

namespace ProductosAPI.Repositories;

// El repositorio es la UNICA clase que habla con la base de datos.
// Aqui usamos ADO.NET: SqlConnection, SqlCommand, SqlDataReader y parametros.
public class ProductoRepository : IProductoRepository
{
    private readonly Conexion _conexion;

    public ProductoRepository(Conexion conexion)
    {
        _conexion = conexion;
    }

    // PUNTO 2: CONSULTAS -> traer todos los productos
    public List<Producto> ObtenerTodos()
    {
        var lista = new List<Producto>();

        try
        {
            using var conexion = _conexion.ObtenerConexion();        // 1. crear conexion
            conexion.Open();                                            // 2. abrir

            var sql = "SELECT Id, Nombre, Precio, Stock FROM Productos";
            using var comando = new SqlCommand(sql, conexion);          // 3. crear comando

            using var lector = comando.ExecuteReader();                 // 4. ejecutar
            while (lector.Read())                                       // 5. leer fila por fila
            {
                lista.Add(new Producto
                {
                    Id = (int)lector["Id"],
                    Nombre = (string)lector["Nombre"],
                    Precio = (decimal)lector["Precio"],
                    Stock = (int)lector["Stock"]
                });
            }
        }                                                               // 6. el using cierra todo
        catch (SqlException ex)
        {
            Console.WriteLine("Error al consultar productos: " + ex.Message);
        }

        return lista;
    }

    // PUNTO 7: BUSCAR -> un producto por su Id
    public Producto? ObtenerPorId(int id)
    {
        try
        {
            using var conexion = _conexion.ObtenerConexion();
            conexion.Open();

            var sql = "SELECT Id, Nombre, Precio, Stock FROM Productos WHERE Id = @Id";
            using var comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@Id", id);                 // parametro SQL

            using var lector = comando.ExecuteReader();
            if (lector.Read())
            {
                return new Producto
                {
                    Id = (int)lector["Id"],
                    Nombre = (string)lector["Nombre"],
                    Precio = (decimal)lector["Precio"],
                    Stock = (int)lector["Stock"]
                };
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error al buscar producto: " + ex.Message);
        }

        return null; // no se encontro
    }

    // PUNTO 3: INSERT -> guardar un producto nuevo y regresar su Id
    public int Insertar(Producto producto)
    {
        try
        {
            using var conexion = _conexion.ObtenerConexion();
            conexion.Open();

            // OUTPUT INSERTED.Id regresa el Id que SQL Server le asigno
            var sql = @"INSERT INTO Productos (Nombre, Precio, Stock)
                        OUTPUT INSERTED.Id
                        VALUES (@Nombre, @Precio, @Stock)";
            using var comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@Nombre", producto.Nombre);
            comando.Parameters.AddWithValue("@Precio", producto.Precio);
            comando.Parameters.AddWithValue("@Stock", producto.Stock);

            return (int)comando.ExecuteScalar();                        // un solo valor: el Id
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error al insertar producto: " + ex.Message);
            return 0; // 0 = no se pudo insertar
        }
    }

    // PUNTO 4: ACTUALIZAR -> cambiar los datos de un producto
    public bool Actualizar(Producto producto)
    {
        try
        {
            using var conexion = _conexion.ObtenerConexion();
            conexion.Open();

            var sql = @"UPDATE Productos
                        SET Nombre = @Nombre, Precio = @Precio, Stock = @Stock
                        WHERE Id = @Id";
            using var comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@Id", producto.Id);
            comando.Parameters.AddWithValue("@Nombre", producto.Nombre);
            comando.Parameters.AddWithValue("@Precio", producto.Precio);
            comando.Parameters.AddWithValue("@Stock", producto.Stock);

            int filas = comando.ExecuteNonQuery();                      // cuantas filas cambiaron
            return filas > 0;
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error al actualizar producto: " + ex.Message);
            return false;
        }
    }

    // PUNTO 5: ELIMINAR -> borrar un producto por su Id
    public bool Eliminar(int id)
    {
        try
        {
            using var conexion = _conexion.ObtenerConexion();
            conexion.Open();

            var sql = "DELETE FROM Productos WHERE Id = @Id";
            using var comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddWithValue("@Id", id);

            int filas = comando.ExecuteNonQuery();
            return filas > 0;
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error al eliminar producto: " + ex.Message);
            return false;
        }
    }
}
