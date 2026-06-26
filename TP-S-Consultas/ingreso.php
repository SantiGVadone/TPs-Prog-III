<?php
$servername = "localhost";
$username = "root";
$password = "root";
$dbname = "miBD";


$tipoDoc = $_POST['tipo_doc'];
$doc = $_POST['documento'];
$user = $_POST['usuario'];
$pass = $_POST['password'];

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
  die("Error en la conexión: " . $conn->connect_error);
}

$sql = "SELECT documento FROM usuarios WEHERE usuario = ".$user." AND password = ".$pass."";

if ($conn->query($sql) === TRUE) {
        echo "LogIn exitoso, redirigiendo a su resumen";  
                
        $conn->close();

        header("Location: resumen.php");
        exit();
} else{
    die("Error en el inicio de sesion: " . $conn->error);
}

$conn->close();

exit();
?>
