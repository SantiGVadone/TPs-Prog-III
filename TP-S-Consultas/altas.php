<?php
$servername = "db";
$username = "root";
$password = "abcde1234";
$dbname = "mi_banco_db";

$tipoDoc = $_POST['tipo_doc'];
$doc = $_POST['documento'];
$nombre = $_POST['nombre'];
$apellido = $_POST['apellido'];
$nacimiento = $_POST['fecha_nacimiento'];
$email = $_POST['email'];
$usuario = $_POST['usuario'];
$passA = $_POST['passwordA'];
$passB = $_POST['passwordB'];

if($passA != $passB){
    die("Las contraseñas tienen que ser iguales");
}

$pass = $passA;

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("Error en la conexión: " . $conn->connect_error);
}

$sqlCheckUsuario = "SELECT usuario, password FROM usuarios WHERE documento = '$doc'";
$resultadoCheck = $conn->query($sqlCheckUsuario);

if ($resultadoCheck && $resultadoCheck->num_rows > 0) {
    $usuarioExistente = $resultadoCheck->fetch_assoc();
    
    if ($usuarioExistente['usuario'] !== null && $usuarioExistente['password'] !== null) {
        die("Error: Este documento ya posee una cuenta web activa. Intentá iniciar sesión.");
    }
}

$sql1 = "SELECT num_cuenta FROM tarjetas WHERE dni_titular = '$doc'";
$resultadoTarjeta = $conn->query($sql1);

if($resultadoTarjeta) {
    if($resultadoTarjeta->num_rows > 0){
        $sql2 = "UPDATE usuarios SET usuario = '$usuario', password = '$pass' WHERE documento = '$doc'";
        
        if($conn->query($sql2) === TRUE){
            $conn->close();
            header("Location: ingreso.html");
            exit();
        } else {
            die("Error al activar la cuenta de usuario: " .$conn->error);
        }
    } else{
        die("Error: No podes registrarte. No tenes ninguna tarjeta emitida con el DNI: " .$doc);
    }
} else{
    die("Error en la consulta de verificación: " . $conn->error);
}

$conn->close();
exit();
?>