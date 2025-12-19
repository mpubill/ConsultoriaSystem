# ConsultoriaSystem
1. Descripción General

ConsultoriaSystem es una API desarrollada con .NET 6 Web API como parte de una prueba técnica. El sistema permite gestionar consultores, paquetes de servicio y relaciones entre ambos, con autenticación JWT, validaciones, paginación, reportes y pruebas unitarias.

3. Tecnologías Utilizadas
- .NET 6 Web API
- SQL Server 
- Autenticación JWT + Roles (Admin / User)
- Repository Pattern + Service Layer
- FluentValidation
- Swagger
- xUnit + Moq para pruebas unitarias

3. Base de Datos y Stored Procedures
La base de datos se compone de 4 tablas: Usuarios, Consultores, PaquetesServicio y ConsultorPaquete. Todas las operaciones CRUD y reportes se ejecutan mediante stored procedures.
Tablas principales:
- Usuarios
- Consultores
- PaquetesServicio
- ConsultorPaquete
Stored Procedures incluidos:
- sp_Usuarios_Login
- sp_Consultores_Insert / Update / Delete / GetAll / GetById
- sp_Paquetes_Insert / Update / Delete / GetAll / GetById
- sp_ConsultorPaquete_Asignar / Desasignar
- sp_Reporte_PaquetesPorArea / ConsultoresTopFacturacion

En el repositorio encontrará el script que le permitira obtener la BD con el esquema completo y data
  
4. Cómo Levantar el Proyecto
- Configurar la cadena de conexión en appsettings.json (Se ha usado SQL EXPRESS con Windows Auth)
- Ejecutar el script SQL de la BD
- Compilar y ejecutar el proyecto 

5. Autenticación JWT
Para usar los endpoints protegidos, primero ejecuta el login para obtener un token.

POST /api/v1/Auth/login
{
  "email": "admin@demo.com",
  "password": "Admin.123"
}

Luego copia el token y agréguelo en Swagger:
Bearer eyJhbGc…

6. Pruebas en Postman (Consultores + Auth)
Ejemplo de prueba en Postman:
Login → obtener token
Agregar token en Authorization → Bearer Token
Crear consultor (solo Admin):

POST /api/v1/consultores
{
  "nombre": "Raúl",
  "areaEspecializacion": "Backend",
  "tarifaHora": 40,
  "emailCorporativo": "raul@empresa.com"
}

7. Paginación en Reportes
Los reportes permiten la paginación con valores por defecto o diferentes. Tambien permite la filtración por campos

8. Pruebas Unitarias
El proyecto ConsultoriaSystem.Test contiene pruebas unitarias usando xUnit y Moq. Incluye validaciones de creación, actualización y eliminación de consultores.
Ejecutar pruebas unitarias: Clic sobre la pestaña Prueba y luego en Ejecutar todas las pruebas

9. Pruebas en postman
En el repositorio se adjunta un archivo json que debera ejecutarse con Postman, este contiene pruebas de endpoints para la auth, select de consultores y post de consultores.
Descargar el archivo y abrirlo es Postman, los valores para prueba ya han quedado guardados pero pueden ser cambiados en base a cada prueba necesaria.
