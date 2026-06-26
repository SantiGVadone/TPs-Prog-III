<?php
session_start();
session_destroy(); // Borra todas las variables de sesion
header("Location: ingreso.html"); // Lo manda al login de una
exit();
?>