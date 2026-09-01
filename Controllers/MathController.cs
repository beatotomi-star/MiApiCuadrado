using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace MiApiCuadrado.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MathController : ControllerBase
    {
        // Tu cadena de conexión real de Somee
        private const string ConnectionString = "workstation id=MiApiCuadradoDB.mssql.somee.com;packet size=4096;user id=beatotomi_SQLLogin_1;pwd=1cm8ihtb44;data source=MiApiCuadradoDB.mssql.somee.com;persist security info=False;initial catalog=MiApiCuadradoDB;TrustServerCertificate=True";

        // Tarea 1: Cuadrado matemático
        [HttpGet("cuadrado")]
        public IActionResult ObtenerCuadrado([FromQuery] int numero)
        {
            if (numero < 0)
            {
                return BadRequest(new { mensaje = "El número no puede ser negativo." });
            }

            int resultado = numero * numero;
            return Ok(new { resultado = resultado });
        }

        // Tarea 2: Retorna los datos reales de Somee usando DAPPER en formato JSON alineado
        [HttpGet("productos")]
        public async Task<IActionResult> ObtenerProductos()
        {
            using var connection = new SqlConnection(ConnectionString);
            
            // Dapper realiza la consulta a SQL Server aquí
            var sql = "SELECT Id, Nombre, Precio, Stock FROM Productos;";
            var productos = await connection.QueryAsync<Producto>(sql);

            // Formateador para alinear el JSON hacia abajo automáticamente
            var opcionesJson = new JsonSerializerOptions 
            { 
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            
            string jsonAlineado = JsonSerializer.Serialize(productos, opcionesJson);
            return Content(jsonAlineado, "application/json; charset=utf-8");
        }
    }

    // Modelo que mapea exactamente las columnas de tu tabla en Somee (sin logos)
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}
