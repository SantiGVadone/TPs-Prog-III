<?php

session_start();

$servername = "db";
$username = "root";
$password = "abcde1234";
$dbname = "mi_banco_db";


$tipoDoc = $_POST['tipo_doc'];
$doc = $_POST['documento'];
$user = $_POST['usuario'];
$pass = $_POST['password'];

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
  die("Error en la conexión: " . $conn->connect_error);
}

$sql = "SELECT documento FROM usuarios WHERE usuario = '$user' AND password = '$pass' AND documento = '$doc'";
$resultadoLogin = $conn->query($sql);

if ($resultadoLogin) {
  if($resultadoLogin->num_rows > 0){
    $fila = $resultadoLogin-> fetch_assoc();
    
    $_SESSION['documento'] = $fila['documento'];

    $conn->close();
    header("Location: resumen.php");
    exit();
  } else {
        die("Error: Usuario o contraseña incorrectos.");
    }
} else {
    die("Error en la consulta de autenticación: " . $conn->error);
}

$conn->close();
?>