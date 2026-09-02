# Diploma

Trabajo final de Ingeniería de Software. Aplicación de escritorio para administrar usuarios, perfiles y permisos, con mecanismos de seguridad, auditoría e integridad de datos.

## Funcionalidades

- Inicio de sesión y gestión de usuarios.
- Gestión de roles, familias y patentes mediante el patrón Composite.
- Cambio de contraseña, bloqueo y desbloqueo de usuarios.
- Bitácora de eventos.
- Verificación y reparación de integridad mediante dígitos verificadores.
- Cambio de idioma con archivos de recursos en español e inglés.
- Respaldo y restauración de la base de datos.

## Arquitectura

La solución está organizada en capas:

```text
TpIngSoftware/
├── BE/                Entidades de negocio
├── BLL/               Reglas y validaciones de negocio
├── DAL/               Acceso a SQL Server
├── Servicios/         Seguridad, sesión, idiomas e integridad
├── TpIngSoftware/     Interfaz Windows Forms
└── Instalador_33ZS/   Proyecto de instalador
```

## Requisitos

- Visual Studio con desarrollo de escritorio para .NET Framework 4.8.
- SQL Server LocalDB o SQL Server Express.
- SQL Server Management Studio, recomendado para ejecutar y revisar los scripts.

## Puesta en marcha

1. Clonar el repositorio.
2. Abrir `TpIngSoftware/TpIngSoftware.sln` en Visual Studio.
3. Ejecutar `BD.sql` en SQL Server. El script crea la base `TpIngSoftware` y su esquema.
4. Ejecutar `QueryCargarDatosIniciales.sql` para cargar los datos de prueba, roles y permisos iniciales.
5. Restaurar paquetes NuGet si Visual Studio lo solicita y compilar la solución.
6. Ejecutar el proyecto `TpIngSoftware`.

> `QueryCargarDatosIniciales.sql` elimina los datos existentes de las tablas de seguridad antes de cargar los datos de prueba. No debe ejecutarse sobre una base con información que se quiera conservar.

## Base de datos y conexión

La conexión usa autenticación integrada de Windows. La aplicación intenta conectarse primero a:

```text
(localdb)\MSSQLLocalDB
```

Como alternativa, también puede usar `.\SQLEXPRESS`. La cadena de conexión se resuelve en la capa `DAL`, por lo que no se almacenan contraseñas de base de datos en el código.

El esquema incluye las tablas `Usuario`, `Rol`, `Patente`, `Familia`, sus relaciones, `Evento` y `DV`.

## Estado actual

El proyecto ya cuenta con la base de seguridad y administración. El siguiente paso es incorporar un módulo de negocio completo y vincular sus operaciones con permisos, bitácora e integridad de datos.
