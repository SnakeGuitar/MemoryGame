using System;
using System.Data.SqlClient;
using System.Linq;

namespace Server.AuthenticationService
{
    public class ConnectionTest
    {
        public static void TestConnection()
        {
            // Prueba 1: Conexión directa con SQL Client
            Console.WriteLine("=== PRUEBA 1: Conexión SQL directa ===");
            TestDirectConnection();

            // Prueba 2: Conexión con Entity Framework
            Console.WriteLine("\n=== PRUEBA 2: Conexión Entity Framework ===");
            TestEntityFrameworkConnection();

            // Prueba 3: Verifica la cadena de conexión
            Console.WriteLine("\n=== PRUEBA 3: Cadena de conexión ===");
            PrintConnectionString();
        }

        private static void TestDirectConnection()
        {
            string connectionString = @"Server=localhost\SQLEXPRESS;Database=memoryGame;Integrated Security=true;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Console.WriteLine("✓ Conexión SQL directa: EXITOSA");

                    // Verifica que la tabla existe
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM usuario", conn))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        Console.WriteLine($"✓ Tabla 'usuario' existe con {count} registros");
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error: {ex.Message}");
                Console.WriteLine($"✗ Inner: {ex.InnerException?.Message}");
            }
        }

        private static void TestEntityFrameworkConnection()
        {
            try
            {
                using (var db = new memoryGameEntities())
                {
                    // Intenta obtener la conexión
                    db.Database.Connection.Open();
                    Console.WriteLine("✓ Conexión Entity Framework: EXITOSA");

                    // Prueba una consulta simple
                    var count = db.usuario.Count();
                    Console.WriteLine($"✓ Consulta EF exitosa: {count} usuarios");

                    db.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error: {ex.Message}");
                Console.WriteLine($"✗ Inner Exception 1: {ex.InnerException?.Message}");
                Console.WriteLine($"✗ Inner Exception 2: {ex.InnerException?.InnerException?.Message}");
                Console.WriteLine($"✗ Inner Exception 3: {ex.InnerException?.InnerException?.InnerException?.Message}");
            }
        }

        private static void PrintConnectionString()
        {
            try
            {
                using (var db = new memoryGameEntities())
                {
                    var connString = db.Database.Connection.ConnectionString;
                    Console.WriteLine($"Connection String:\n{connString}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ No se pudo obtener: {ex.Message}");
            }
        }
    }
}