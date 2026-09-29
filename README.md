# Diploma

Trabajo final de Ingeniería de Software. Aplicación de escritorio para gestionar la atención presencial de una barbería, con usuarios, permisos, auditoría e integridad de datos.

## Funcionalidades

- Inicio de sesión y gestión de usuarios.
- Gestión de roles, familias y patentes mediante el patrón Composite.
- Cambio de contraseña, bloqueo y desbloqueo de usuarios.
- Bitácora de eventos.
- Verificación y reparación de integridad mediante dígitos verificadores.
- Cambio de idioma con archivos de recursos en español e inglés.
- Respaldo y restauración de la base de datos.
- Puestos Dueño, Recepcionista y Barbero, cada uno con cuenta y patentes propias.
- Identificación y alta de clientes, configuración de barberos y comisiones, catálogo, precios y stock.
- Registro conjunto de atención, cobro, comisión y consumo de insumos. Historial propio del barbero y consulta de actividad para el dueño.

El flujo es presencial. No hay turnos ni reservas. PN2 (compras y reposición de insumos) sigue pendiente.

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

En una base nueva aparece una pantalla de configuración antes del inicio de sesión. Ingresá DNI, nombre, apellido, email y contraseña del administrador (12 a 50 caracteres). Después se cargan automáticamente cinco clientes y cuatro cuentas de demostración. La carga se confirma en una única transacción mediante BLL, mappers y procedimientos almacenados, con PBKDF2, bitácora y DV. Una marca en la base evita volver a crear estos datos en cada inicio. En una base existente se agregan una sola vez, sin modificar usuarios ni clientes previos.

| Puesto | Usuario | Contraseña |
| --- | --- | --- |
| Recepcionista | `recepcion.demo@harlem.local` | `DemoHarlem2026!` |
| Barbero Mateo | `barbero.mateo@harlem.local` | `DemoHarlem2026!` |
| Barbero Tomás | `barbero.tomas@harlem.local` | `DemoHarlem2026!` |
| Dueño | `dueno.demo@harlem.local` | `DemoHarlem2026!` |

Los clientes de ejemplo son Sofía Ramírez, Julián Torres, Camila Morales, Nicolás Castro y Martina Silva. Los clientes no ingresan al sistema y no tienen usuario ni contraseña. Los dos barberos se crean activos con una comisión de ejemplo del 50 %. Estas credenciales son públicas y sirven sólo para mostrar el sistema; cambialas antes de usar datos reales.

Para usar PN1, iniciá sesión como Administrador. En **Catálogo y stock**, asigná precios a los servicios, revisá o editá la cantidad fija de cada insumo por atención y cargá el stock inicial con un motivo de ajuste. También podés crear un servicio nuevo eligiendo su nombre, precio e insumos existentes; se exige al menos un insumo con cantidad positiva. En **Barberos**, configurá la comisión y activá el perfil del usuario Barbero. Después podés ingresar como Recepcionista, buscar o registrar un cliente y cargar una atención con su cobro. La pantalla muestra si alcanza el stock antes de realizar el servicio y el SP vuelve a verificarlo al registrar el cobro. El barbero ingresa con su cuenta para consultar sólo sus atenciones y comisiones. También podés crear usuarios con rol Dueño desde **Usuarios**; ese puesto gestiona la operación y las cuentas de trabajo, pero no administra perfiles ni cuentas de Administrador.

`RegistrarAtencion` ya está asignada a Recepcionista y se exige al registrar en BLL y en el procedimiento SQL. La Recepcionista también puede buscar y registrar clientes, consultar el catálogo y ver el stock mediante `ConsultarStock`. Sólo `GestionarCatalogo` habilita editar servicios y sólo `GestionarStock` habilita ajustar existencias. `ConsultarAtencionesPropias` permite al Barbero ver su historial y sus comisiones; su consulta SQL y su modelo no incluyen el precio cobrado. `ConsultarAtencionesGenerales` reserva el reporte de ingresos para Dueño y Administrador. Las altas y cambios de usuarios o perfiles, la consulta de bitácora y las operaciones de respaldo comprueban sus patentes también en BLL, además de ocultar las acciones no permitidas en pantalla.

Para una instalación automatizada se pueden proporcionar `TPINGSOFTWARE_ADMIN_DNI`, `TPINGSOFTWARE_ADMIN_NOMBRE`, `TPINGSOFTWARE_ADMIN_APELLIDOS`, `TPINGSOFTWARE_ADMIN_EMAIL` y `TPINGSOFTWARE_ADMIN_PASSWORD` en el entorno del proceso. Si falta alguna, se abre el formulario. Retirá la variable con la contraseña del entorno desde el que lanzaste la aplicación cuando termine la instalación. La contraseña del administrador la elegís vos y no se cambia al cargar los ejemplos.

> `BD.sql` y `QueryCargarDatosIniciales.sql` sólo se usan al crear una base nueva. El script de datos iniciales elimina registros de seguridad; no lo ejecutes sobre una base existente.

## Base de datos y conexión

La conexión predeterminada usa autenticación integrada de Windows con:

```text
(localdb)\MSSQLLocalDB
```

La instancia y el nombre de base se configuran en `App.config`. Para pruebas o instalaciones se puede usar la variable de entorno `TPINGSOFTWARE_CONNECTION_STRING`, que tiene prioridad. La única contraseña incluida en el código es la de las cuentas públicas de demostración; la contraseña del administrador se configura al instalar.

El máximo de intentos de inicio de sesión (`MaxIntentosLogin`) y el costo de PBKDF2 (`PasswordHashIterations`) se configuran en `App.config`. Los intentos fallidos se guardan en `LoginIntento`, por lo que cerrar y abrir la aplicación no reinicia el contador. Un acceso correcto o el desbloqueo de la cuenta lo reinicia. Las contraseñas nuevas usan PBKDF2 con sal individual. Los hashes SHA-256 anteriores se verifican para conservar el acceso y se actualizan al primer inicio de sesión correcto.

El esquema incluye las tablas de seguridad `Usuario`, `Rol`, `Patente`, `Familia`, sus relaciones, `Evento`, `DV` y `LoginIntento`. PN1 añade `Cliente`, `BarberoPerfil`, `ServicioCatalogo`, `Insumo`, `ServicioConsumo`, `MedioPago`, `AjusteStock`, `Atencion`, `Cobro` y `ConsumoAtencion` mediante las migraciones `005` a `007`; `008` incorpora el guardado transaccional de servicios y su consumo de insumos; `009` comprueba la patente de registro de atenciones, directa o heredada por familias, dentro del SP; `010` registra si se aplicó la carga de demostración; `011` agrega el permiso de consulta de stock y actualiza los dígitos verificadores; `012` retira el precio de la consulta de historial propio. La operación de atención usa un procedimiento almacenado transaccional con rollback, validaciones de permisos, precio y stock, y un identificador único que evita duplicar una operación al reintentarla. Los precios, comisiones, consumos y existencias se configuran en la base; los valores de ejemplo del catálogo se crean sólo durante la migración. Los totales del reporte de atenciones llegan desde el SP a través de mapper y BLL.

Las operaciones ordinarias de seguridad pasan por procedimientos almacenados de las migraciones `002_ProcedimientosSeguridad`, `003_IntegridadFamilias` y `004_IntentosLogin`; las escrituras de usuarios y perfiles, sus dígitos verificadores y sus eventos de bitácora se confirman conjuntamente. Respaldo, restauración y el registro de migraciones son operaciones de infraestructura que requieren comandos SQL desde DAL o el inicializador.

Si LocalDB informa que la instancia está detenida mientras SQL Server Management Studio mantiene una conexión activa, la cadena por alias puede fallar. Guardá y cerrá SSMS antes de reiniciar LocalDB; luego verificá la conexión con `(localdb)\MSSQLLocalDB`. No fijes en el repositorio un nombre de canalización `LOCALDB#...`, porque cambia al reiniciar la instancia. La variable `TPINGSOFTWARE_CONNECTION_STRING` permite apuntar temporalmente a la conexión activa durante el diagnóstico.
