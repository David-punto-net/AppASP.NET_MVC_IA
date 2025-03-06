# AppVentas
Repositorio para aplicacion ASP.NET 
# Proyecto ASP.NET MVC con .NET 8

Este proyecto es una aplicación web desarrollada con ASP.NET Core MVC y .NET 8. Implementa funcionalidades de autenticación, gestión de usuarios, administración de productos y procesamiento de pedidos y un agente IA.

Características Principales

Autenticación y Autorización: Implementación de login, logout, registro de usuarios, recuperación de contraseña y bloqueo por intentos fallidos.

Gestión de Usuarios: Administración de roles y permisos, confirmación de correo y seguridad en accesos.

Carro de Compras: Agregado de productos al carrito, gestión de pedidos y pagos.

Administración: Panel de control para gestionar productos, categorías, países, estados y ciudades.

Interfaz Dinámica: Uso de DataTables, ventanas modales y mejoras en la UI con Bootstrap.

Persistencia de Datos: Implementación con Entity Framework Core y migraciones para la base de datos.

Despliegue en Azure: Configuración para publicar la aplicación en la nube.

Requisitos Previos

.NET 8 SDK

SQL Server o LocalDB

Visual Studio 2022 o superior

Cuenta en Azure (opcional para despliegue)

Instalación y Configuración

Clona el repositorio en tu máquina local:

git clone https://github.com/tu_usuario/tu_repositorio.git

Navega al directorio del proyecto:

cd tu_repositorio

Restaura las dependencias:

dotnet restore

Configura la base de datos en appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Shopping;Trusted_Connection=True;MultipleActiveResultSets=true"
}

Aplica las migraciones y genera la base de datos:

dotnet ef database update

Ejecuta la aplicación:

dotnet run

Despliegue en Azure

Publica la aplicación desde Visual Studio o mediante Azure CLI.

Configura la base de datos en Azure SQL Database.

Ajusta los appsettings.json con la conexión de producción.
