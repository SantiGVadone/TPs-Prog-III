using System;
using System.ComponentModel.Design;
using System.Text.Json;
using Microsoft.VisualBasic.FileIO;
using MySql.Data.MySqlClient; 

namespace Progra3Card.Administrativo
{
    class Program
    {
        private static string connectionString = "Server=localhost;Database=mi_banco_db;Uid=root;Pwd=;";

        static void Main(string[] args)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("    SISTEMA ADMINISTRATIVO PROGRA3CARD   ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Emitir Nueva Tarjeta (Alta de Cliente)");
                Console.WriteLine("2. Listar Tarjetas");
                Console.WriteLine("3. Ver Detalle de una Tarjeta / Cliente");
                Console.WriteLine("4. Eliminar Tarjeta (Baja de Sistema)");
                Console.WriteLine("5. Emitir Nueva Liquidación Mensual");
                Console.WriteLine("6. Salir");
                Console.WriteLine("========================================");
                Console.Write("Seleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1": MenuEmitirTarjeta(); break;
                    case "2": MenuListarTarjetas(); break;
                    case "3": MenuVerDetalleTarjeta(); break;
                    case "4": MenuEliminarTarjeta(); break;
                    case "5": MenuEmitirLiquidacion(); break;
                    case "6": salir = true; break;
                    default:
                        Console.WriteLine("Opción no válida. Presione una tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }


        static void MenuEmitirTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- EMITIR NUEVA TARJETA (ALTA DE CLIENTE) ---");

            Console.Write("Tipo de documento (DNI / PASAPORTE): ");
            string tipoDoc = Console.ReadLine().ToUpper();

            Console.Write("Número de documento: ");
            string documento = Console.ReadLine();

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Apellido: ");
            string apellido = Console.ReadLine();

            Console.Write("Fecha de nacimiento (YYYY-MM-DD): ");
            string fechaNac = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Número de tarjeta (16 dígitos): ");
            string nroTarjeta = Console.ReadLine();

            Console.WriteLine("Seleccione el banco emisor:");
            Console.WriteLine("  1. Banco Nación");
            Console.WriteLine("  2. Banco Provincia");
            Console.WriteLine("  3. Banco Galicia");
            Console.WriteLine("  4. Banco Santander");
            Console.WriteLine("  5. Banco BBVA");
            Console.WriteLine("  6. Banco Macro");
            Console.Write("Opción: ");
            string opBanco = Console.ReadLine();

            string bancoEmisor = "";
            if (opBanco == "1") bancoEmisor = "Banco Nación";
            else if (opBanco == "2") bancoEmisor = "Banco Provincia";
            else if (opBanco == "3") bancoEmisor = "Banco Galicia";
            else if (opBanco == "4") bancoEmisor = "Banco Santander";
            else if (opBanco == "5") bancoEmisor = "Banco BBVA";
            else if (opBanco == "6") bancoEmisor = "Banco Macro";
            else
            {
                Console.WriteLine("Banco no válido. Operación cancelada.");
                Console.ReadKey();
                return;
            }

            Console.Write("Saldo inicial: ");
            decimal saldo = Convert.ToDecimal(Console.ReadLine());

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string sqlUsuario = "INSERT INTO usuarios (documento, tipo_doc, nombre, apellido, fecha_nacimiento, email, usuario, password) " +
                                "VALUES (@doc, @tipoDoc, @nombre, @apellido, @fechaNac, @email, NULL, NULL)";
            MySqlCommand cmdU = new MySqlCommand(sqlUsuario, conn);
            cmdU.Parameters.AddWithValue("@doc", documento);
            cmdU.Parameters.AddWithValue("@tipoDoc", tipoDoc);
            cmdU.Parameters.AddWithValue("@nombre", nombre);
            cmdU.Parameters.AddWithValue("@apellido", apellido);
            cmdU.Parameters.AddWithValue("@fechaNac", fechaNac);
            cmdU.Parameters.AddWithValue("@email", email);
            cmdU.ExecuteNonQuery();

            string sqlTarjeta = "INSERT INTO tarjetas (numero_tarjeta, banco_emisor, estado, saldo, dni_titular) " +
                                "VALUES (@nro, @banco, 'Activa', @saldo, @doc)";
            MySqlCommand cmdT = new MySqlCommand(sqlTarjeta, conn);
            cmdT.Parameters.AddWithValue("@nro", nroTarjeta);
            cmdT.Parameters.AddWithValue("@banco", bancoEmisor);
            cmdT.Parameters.AddWithValue("@saldo", saldo);
            cmdT.Parameters.AddWithValue("@doc", documento);
            cmdT.ExecuteNonQuery();

            conn.Close();

            Console.WriteLine("\nCliente y tarjeta registrados correctamente.");
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }




        // Funciones a completar:
        static void MenuListarTarjetas()
        {
            Console.Clear();
            Console.WriteLine("--- LISTADO GENERAL DE TARJETAS ---");
            Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}", "Nro Cuenta", "Nro Tarjeta", "Banco Emisor", "DNI Titular");
            Console.WriteLine("----------------------------------------------------------------------");

            // === A realizar ===
            // Aquí deben implementar un SELECT sobre la tabla 'tarjetas'
            // para recorrer las filas e imprimirlas en la consola.
            
            ObtenerYMostrarTarjetas();

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuVerDetalleTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- DETALLE DE TARJETA Y CLIENTE ---");
            Console.Write("Ingrese el Número de Cuenta a consultar: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());

            // === A realizar ===
            // Aquí deben realizar un SELECT con un JOIN entre 'tarjetas' y 'usuarios' 
            // filtrando por el numCuenta para traer todos los campos (Nombre, Apellido, Email, Saldo, etc.)
            
            MostrarDetalleCompleto(numCuenta);

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuEliminarTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- ELIMINAR TARJETA DEL SISTEMA ---");
            Console.Write("Ingrese el Número de Cuenta de la tarjeta a dar de baja: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠️ ADVERTENCIA: Se eliminará la tarjeta, sus liquidaciones y los datos de acceso web vinculados.");
            Console.ResetColor();
            Console.Write("¿Está seguro de continuar? (S/N): ");
            
            if (Console.ReadLine().ToUpper() == "S")
            {
                // === A realizar ===
                // Aquí deben ejecutar un DELETE sobre la tabla 'tarjetas' donde num_cuenta = numCuenta.
                // Como definimos ON DELETE CASCADE en la base de datos, las liquidaciones se borrarán solas.
                // Opcional: Evaluar si también eliminan al usuario de la tabla 'usuarios' o si lo mantienen.
                
                bool exito = DarDeBajaTarjeta(numCuenta);

                if (exito)
                    Console.WriteLine("\nTarjeta eliminada correctamente del sistema.");
                else
                    Console.WriteLine("\nError al intentar eliminar la tarjeta. Verifique el número de cuenta.");
            }
            else
            {
                Console.WriteLine("\nOperación cancelada.");
            }

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }


        // =========================================================================
        // MÉTODOS BASE QUE DEBEN COMPLETAR CON LA LÓGICA 
        // =========================================================================

        static void ObtenerYMostrarTarjetas()
        {
            // Completar 
            // Ejemplo de impresión dentro del bucle: 
            // Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}", reader["num_cuenta"], reader["numero_tarjeta"], ...);
            {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string sql = "SELECT num_cuenta, numero_tarjeta, banco_emisor, dni_titular FROM tarjetas";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}",
                    reader["num_cuenta"],
                    reader["numero_tarjeta"],
                    reader["banco_emisor"],
                    reader["dni_titular"]);
            }

            conn.Close();
        }
        }



        

        static void MostrarDetalleCompleto(int cuenta)
        {
            // Completar haciendo un SELECT con JOIN de usuarios y tarjetas WHERE num_cuenta = @cuenta
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string sql = @"SELECT t.num_cuenta, t.numero_tarjeta, t.banco_emisor, t.estado, t.saldo,
                        u.documento, u.tipo_doc, u.nombre, u.apellido, u.fecha_nacimiento, u.email, u.usuario
                        FROM tarjetas t 
                        INNER JOIN usuarios u ON t.dni_titular = u.documento WHERE t.num_cuenta = @cuenta";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@cuenta", cuenta);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine("\n  Nro. Cuenta   : " + reader["num_cuenta"]);
                Console.WriteLine("  Nro. Tarjeta  : " + reader["numero_tarjeta"]);
                Console.WriteLine("  Banco Emisor  : " + reader["banco_emisor"]);
                Console.WriteLine("  Estado        : " + reader["estado"]);
                Console.WriteLine("  Saldo         : $ " + reader["saldo"]);
                Console.WriteLine("  Documento     : " + reader["tipo_doc"] + " " + reader["documento"]);
                Console.WriteLine("  Titular       : " + reader["nombre"] + " " + reader["apellido"]);
                Console.WriteLine("  Nacimiento    : " + reader["fecha_nacimiento"]);
                Console.WriteLine("  Email         : " + reader["email"]);
                Console.WriteLine("  Usuario Web   : " + (reader["usuario"] == DBNull.Value ? "(sin activar)" : reader["usuario"].ToString()));
            }
            else
            {
                Console.WriteLine("No se encontró ninguna tarjeta con ese número de cuenta.");
            }

            conn.Close();
        }

        static bool DarDeBajaTarjeta(int cuenta)
        {
            // Completar usando un DELETE FROM tarjetas WHERE num_cuenta = @cuenta
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string sql = "DELETE FROM tarjetas WHERE num_cuenta = @cuenta";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@cuenta", cuenta);

            int filas = cmd.ExecuteNonQuery();

            conn.Close();

            return filas > 0;
        }
    }
}