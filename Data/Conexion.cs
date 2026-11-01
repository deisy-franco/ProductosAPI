using Microsoft.Data.SqlClient;

namespace ProductosAPI.Data;

// Esta clase lee la cadena de conexion de appsettings.json
// y nos da un objeto SqlConnection listo para usarse.
public class Conexion
{
    private readonly string _cadena;

    public Conexion(IConfiguration configuracion)
    {
        _cadena = configuracion.GetConnectionString("DefaultConnection")!;
    }

    public SqlConnection ObtenerConexion()
    {
        return new SqlConnection(_cadena);
    }
}
