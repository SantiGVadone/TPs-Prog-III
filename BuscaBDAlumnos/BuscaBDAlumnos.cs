/*===============================================================================
PROGRAMACIÓN III Conexión Lineal a MySQL 
 
 ⚠️ Antes de correr el proyecto, se debe instalar el driver de MySQL.
 En VSCode ejecutar este comando por terminal:
 dotnet add package MySql.Data --source https://api.nuget.org/v3/index.json
En Visual Studio (Comunity, etc):
Ir a: Herramientas > Administrador de Paquetes NuGet > Administrar paquetes NuGet > MyDql.Data
===============================================================================*/


using System;

// importo todo
using MySqlConnection = MySql.Data.MySqlClient.MySqlConnection;
using MySqlCommand = MySql.Data.MySqlClient.MySqlCommand;
using MySqlDataReader = MySql.Data.MySqlClient.MySqlDataReader;
namespace BuscaBDAlumnos
{
    class Program
    {
        static void Main(string[] args)
        {
            //seteo la conexion a la db
            string connectionString = "Server=localhost;Port=3307;Database=prog3n3;Uid=root;Pwd=abcde1234;";
            Console.WriteLine("Intentando conectar a la base de datos MySQL...");
            // Abrimos la conexión asegurando el cierre de recursos con 'using'.
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();

                    Console.WriteLine("¡Conexión exitosa al servidor de MySQL!\n");


                    Console.WriteLine("Ingrese el numero de legajo a buscar: ");
                    string? legajoInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(legajoInput))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("El legajo ingresado no puede estar vacío.");
                        Console.ResetColor();
                        return;
                    }

                    string consulta = "SELECT nombre, apellido, email, carrera, turno FROM alumnos WHERE legajo = @legajo";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@legajo", legajoInput);

                        using (MySqlDataReader lector = comando.ExecuteReader())
                        {
                            if (!lector.HasRows)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"\n[!] El alumno con legajo '{legajoInput}' no fue encontrado.");
                                Console.ResetColor();
                            } else
                            {
                                
                            }

                            Console.WriteLine("==========================================================================================================");
                            Console.WriteLine("                                           LISTADO DE ALUMNOS (LINEAL)                                    ");
                            Console.WriteLine("==========================================================================================================");
                            Console.WriteLine(string.Format("{0,-10} | {1,-12} | {2,-12} | {3,-32} | {4,-22} | {5,-8}", 
                                "Legajo", "Nombre", "Apellido", "Email", "Carrera", "Turno"));
                            Console.WriteLine("----------------------------------------------------------------------------------------------------------");

                            // Bloque iterativo: leemos fila por fila mientras el lector tenga datos
                            while (lector.Read())
                            {
                                string nombre = lector["nombre"].ToString()??"";
                                string apellido = lector["apellido"].ToString()??"";
                                string email = lector["email"].ToString()??"";
                                string carrera = lector["carrera"].ToString()??"";
                                string turno = lector["turno"].ToString()??"";

                                Console.WriteLine(string.Format("{0,-10} | {1,-12} | {2,-12} | {3,-32} | {4,-22} | {5,-8}", 
                                    legajoInput, nombre, apellido, email, carrera, turno));
                                Console.ReadLine();
                            }
                            Console.WriteLine("==========================================================================================================\n");
                            
                        }
                    }
                }
                
                catch (Exception ex)
                {
                    // Control de errores ante fallas de red, credenciales o servidor apagado
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ocurrió un error al intentar operar con la base de datos:");
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
            }
            

            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}