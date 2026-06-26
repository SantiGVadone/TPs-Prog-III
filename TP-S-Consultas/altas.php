<?php
$servername = "localhost";
$username = "root";
$password = "root";
$dbname = "miBD";

// vienen de cargarDatos.html
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
} else {
    $pass = $passA;
}

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
  die("Error en la conexión: " . $conn->connect_error);
}

$sql = "SELECT num_cuenta FROM tarjetas WEHERE dni_titular = ".$doc."";

if ($conn->query($sql) === TRUE) {
    $sql2 = "UPDATE usuarios SET usuario = '$usuario', password = '$pass' WHERE documento = '$doc'";
    if ($conn->query($sql2) === TRUE){
        echo "Alta exitosa";  
                
        $conn->close();

        header("Location: ingreso.html");
        exit();
    } else{
            die("Error al dar el alta del usuario: " . $conn->error);
        }
} else{
    die("Error al obtener los datos de la tarjeta: " . $conn->error);
}


$conn->close();
exit();
?>