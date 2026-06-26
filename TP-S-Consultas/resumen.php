<?php
session_start();

if (!isset($_SESSION['documento'])) {
    header("Location: ingreso.html");
    exit();
}

$dni_usuario = $_SESSION['documento'];

$servername = "db";
$username = "root";
$password = "abcde1234";
$dbname = "mi_banco_db";

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("Error en la conexión: " . $conn->connect_error);
}


$sql = "SELECT u.nombre, u.apellido, t.numero_tarjeta, t.banco_emisor, t.saldo,
               l.periodo, l.fecha_vencimiento, l.total_a_pagar, l.pago_minimo
        FROM usuarios u
        INNER JOIN tarjetas t ON u.documento = t.dni_titular
        LEFT JOIN liquidaciones l ON t.num_cuenta = l.num_cuenta
        WHERE u.documento = '$dni_usuario'
        ORDER BY l.periodo DESC";

$resultado = $conn->query($sql);

if (!$resultado) {
    die("Error al procesar el resumen financiero: " . $conn->error);
}

$datos_fijos = null;
if ($resultado->num_rows > 0) {
    
    $datos_fijos = $resultado->fetch_assoc();
    
    $resultado->data_seek(0);
}
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Mis Tarjetas - Resumen de Cuenta</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-100 font-sans antialiased">

    <nav class="bg-blue-600 p-4 text-white shadow-md">
        <div class="container mx-auto flex justify-between items-center">
            <h1 class="text-xl font-bold tracking-wider">Programacion III - HomeBanking</h1>
            <div class="flex items-center space-x-4">
                <?php if ($datos_fijos): ?>
                    <span class="font-medium text-blue-100">Hola, <?php echo $datos_fijos['nombre'] . " " . $datos_fijos['apellido']; ?></span>
                <?php endif; ?>
                <a href="logout.php" class="bg-blue-700 hover:bg-blue-800 px-3 p-1.5 rounded text-sm font-semibold transition">Cerrar Sesión</a>
            </div>
        </div>
    </nav>

    <main class="container mx-auto mt-8 px-4 max-w-5xl">
        
        <?php if ($datos_fijos): ?>
            <div class="bg-white rounded-lg shadow-md p-6 mb-8 max-w-md mx-auto border-t-4 border-blue-500">
                <div class="flex justify-between items-start mb-4">
                    <div>
                        <p class="text-xs uppercase tracking-widest text-gray-400 font-semibold">Banco Emisor</p>
                        <h2 class="text-lg font-bold text-gray-700"><?php echo $datos_fijos['banco_emisor']; ?></h2>
                    </div>
                    <span class="bg-green-100 text-green-800 text-xs px-2.5 py-0.5 rounded font-bold uppercase">Activa</span>
                </div>
                <div class="my-6">
                    <p class="text-sm text-gray-500 tracking-widest font-mono">
                        <?php 
                            $num = $datos_fijos['numero_tarjeta']; 
                            echo substr($num, 0, -12) . "-" . substr($num, 4, -8) . "-" .substr($num, 8, -4) . "-" .substr($num, -4); 
                        ?>
                    </p>
                </div>
                <div class="border-t pt-4 flex justify-between">
                    <div>
                        <p class="text-xs text-gray-400 font-medium">Titular</p>
                        <p class="text-sm font-semibold text-gray-600"><?php echo $datos_fijos['apellido'] . ", " . $datos_fijos['nombre']; ?></p>
                    </div>
                    <div class="text-right">
                        <p class="text-xs text-gray-400 font-medium">Saldo Actual</p>
                        <p class="text-sm font-bold text-gray-800">$<?php echo number_format($datos_fijos['saldo'], 2, ',', '.'); ?></p>
                    </div>
                </div>
            </div>

            <div class="bg-white rounded-lg shadow-md overflow-hidden">
                <div class="bg-gray-50 px-6 py-4 border-b">
                    <h3 class="font-bold text-gray-700 text-lg">Historial de Liquidaciones</h3>
                </div>
                
                <div class="overflow-x-auto">
                    <table class="w-full text-left border-collapse">
                        <thead>
                            <tr class="bg-gray-100 text-gray-600 text-xs uppercase tracking-wider font-semibold border-b">
                                <th class="px-6 py-3">Período</th>
                                <th class="px-6 py-3">Vencimiento</th>
                                <th class="px-6 py-3 text-right">Pago Mínimo</th>
                                <th class="px-6 py-3 text-right">Total a Pagar</th>
                            </tr>
                        </thead>
                        <tbody class="divide-y text-sm text-gray-600">
                            <?php if ($datos_fijos && $datos_fijos['periodo'] !== null): ?>
                                <?php while ($row = $resultado->fetch_assoc()): ?>
                                    <tr class="hover:bg-gray-50 transition">
                                        <td class="px-6 py-4 font-semibold text-gray-900"><?php echo $row['periodo']; ?></td>
                                        <td class="px-6 py-4"><?php echo date("d/m/Y", strtotime($row['fecha_vencimiento'])); ?></td>
                                        <td class="px-6 py-4 text-right font-mono text-gray-700">$<?php echo number_format($row['pago_minimo'], 2, ',', '.'); ?></td>
                                        <td class="px-6 py-4 text-right font-mono font-bold text-blue-600">$<?php echo number_format($row['total_a_pagar'], 2, ',', '.'); ?></td>
                                    </tr>
                                <?php endwhile; ?>
                            <?php else: ?>
                                <tr>
                                    <td colspan="4" class="px-6 py-8 text-center text-gray-400 italic">
                                        No se registraron liquidaciones emitidas para este plástico todavía.
                                    </td>
                                </tr>
                            <?php endif; ?>
                        </tbody>
                    </table>
                </div>
            </div>

        <?php else: ?>
            <div class="bg-yellow-50 border-l-4 border-yellow-400 p-4 rounded shadow">
                <div class="flex">
                    <div class="ml-3">
                        <p class="text-sm text-yellow-700 font-medium">
                            No se encontraron tarjetas ni liquidaciones activas registradas para tu cuenta. Por favor, contactate a la entidad financiera.
                        </p>
                    </div>
                </div>
            </div>
        <?php endif; ?>

    </main>

</body>
</html>
<?php 
$conn->close(); 
?>