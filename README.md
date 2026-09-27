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
├── DAL/               Accesos por módulo y conexión a SQL Server
├── Mappers/           Conversión entre resultados y entidades
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
3. Configurar la cadena `TpIngSoftware` en `TpIngSoftware/TpIngSoftware/App.config` para la instancia local.
4. Restaurar paquetes NuGet si Visual Studio lo solicita y compilar la solución.
5. Ejecutar el proyecto `TpIngSoftware`. Si la base no existe, la aplicación la crea; luego aplica las migraciones pendientes en orden.

En una base nueva aparece una pantalla de configuración antes del inicio de sesión. Ingresá DNI, nombre, apellido, email y contraseña del administrador, y elegí otra contraseña para las dos cuentas de ejemplo `demo1@harlem.local` y `demo2@harlem.local` (rol `Invitado`). Ambas contraseñas deben tener entre 12 y 50 caracteres. Las tres altas se confirman juntas mediante BLL, mappers y SP, con PBKDF2, bitácora y DV. Si cancelás o falla una alta, no queda una cuenta parcial y podés repetir la configuración al volver a iniciar.

Para una instalación automatizada se pueden proporcionar `TPINGSOFTWARE_ADMIN_DNI`, `TPINGSOFTWARE_ADMIN_NOMBRE`, `TPINGSOFTWARE_ADMIN_APELLIDOS`, `TPINGSOFTWARE_ADMIN_EMAIL`, `TPINGSOFTWARE_ADMIN_PASSWORD` y `TPINGSOFTWARE_DEMO_PASSWORD` en el entorno del proceso. Si falta alguna, se abre el formulario. Retirá las variables con contraseñas del entorno desde el que lanzaste la aplicación cuando termine la instalación. El repositorio incluye identidades de ejemplo, pero ninguna contraseña predeterminada.

> `BD.sql` y `QueryCargarDatosIniciales.sql` sólo se usan al crear una base nueva. El script de datos iniciales elimina registros de seguridad; no lo ejecutes sobre una base existente.

## Base de datos y conexión

La conexión predeterminada usa autenticación integrada de Windows con:

```text
(localdb)\MSSQLLocalDB
```

La instancia y el nombre de base se configuran en `App.config`. Para pruebas o instalaciones se puede usar la variable de entorno `TPINGSOFTWARE_CONNECTION_STRING`, que tiene prioridad. No se almacenan contraseñas en el código.

El máximo de intentos de inicio de sesión (`MaxIntentosLogin`) y el costo de PBKDF2 (`PasswordHashIterations`) se configuran en `App.config`. Los intentos fallidos se guardan en `LoginIntento`, por lo que cerrar y abrir la aplicación no reinicia el contador. Un acceso correcto o el desbloqueo de la cuenta lo reinicia. Las contraseñas nuevas usan PBKDF2 con sal individual. Los hashes SHA-256 anteriores se verifican para conservar el acceso y se actualizan al primer inicio de sesión correcto.

El esquema incluye las tablas `Usuario`, `Rol`, `Patente`, `Familia`, sus relaciones, `Evento`, `DV` y `LoginIntento`. Las operaciones ordinarias pasan por procedimientos almacenados de las migraciones `002_ProcedimientosSeguridad`, `003_IntegridadFamilias` y `004_IntentosLogin`; las escrituras de usuarios y perfiles, sus dígitos verificadores y sus eventos de bitácora se confirman conjuntamente. Respaldo, restauración y el registro de migraciones son operaciones de infraestructura que requieren comandos SQL desde DAL o el inicializador.

Si LocalDB informa que la instancia está detenida mientras SQL Server Management Studio mantiene una conexión activa, la cadena por alias puede fallar. Guardá y cerrá SSMS antes de reiniciar LocalDB; luego verificá la conexión con `(localdb)\MSSQLLocalDB`. No fijes en el repositorio un nombre de canalización `LOCALDB#...`, porque cambia al reiniciar la instancia. La variable `TPINGSOFTWARE_CONNECTION_STRING` permite apuntar temporalmente a la conexión activa durante el diagnóstico.
