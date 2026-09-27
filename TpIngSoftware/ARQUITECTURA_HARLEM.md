# Arquitectura de Harlem

## Referencias y alcance

La documentación funcional del Trabajo Final define el alcance de Harlem. La barbería atiende a clientes que se presentan en el local: la recepcionista los identifica o registra allí y, después de prestar el servicio, registra la atención y el cobro. Los procesos son PN1 (prestación y cobro de servicios) y PN2 (compra y reposición de insumos). No se implementan turnos, reservas ni agenda de disponibilidad.

La solución separa presentación, reglas de negocio, transformación de datos y persistencia. Cada módulo nuevo debe mantener reconocible su recorrido entre esas capas y versionar los cambios de esquema junto con los procedimientos almacenados.

## Estado actual de la solución Harlem

La solución usa `TpIngSoftware` como proyecto de interfaz, `Servicios` para seguridad y servicios compartidos, y proyectos separados `BE`, `BLL`, `Mappers` y `DAL`. Sus referencias actuales son:

```text
TpIngSoftware -> BE, BLL, Servicios
BLL           -> BE, Mappers, Servicios
Mappers       -> DAL, Servicios
Servicios     -> BE, DAL
DAL           -> sin referencias a otros proyectos de la solución
BE            -> sin referencias a otros proyectos de la solución
```

El scaffold existente implementa usuarios, roles y permisos, sesión, idiomas, bitácora, respaldo e integridad. `BLL` coordina las operaciones y los dígitos verificadores; los mappers convierten entidades y conjuntos de resultados; `DAL/Security` y `DAL/Integrity` contienen clases de acceso por módulo que invocan procedimientos almacenados a través de `DAL/Persistence/Acceso_33ZS.cs`. `BE` aún no contiene entidades de negocio; algunas entidades heredadas de seguridad están en `Servicios`.

Las contraseñas nuevas usan PBKDF2 con sal individual. El inicio de sesión reconoce los hashes SHA-256 heredados y los reemplaza tras una validación correcta; `Encriptador_33ZS.Hash` sigue usándose para el cálculo de dígitos verificadores. Los parámetros de política ajustables están en `App.config`.

Las migraciones y la inicialización de la base permanecen en `Servicios` como infraestructura del proyecto. La migración `002_ProcedimientosSeguridad.sql` incorpora los SP de la seguridad existente; la `003_IntegridadFamilias.sql` evita borrar familias referenciadas y comprueba ciclos dentro de la transacción; la `004_IntentosLogin.sql` persiste los intentos fallidos y el bloqueo. Sus escrituras usan transacción, `XACT_ABORT`, rollback y propagación de errores. La BLL agrupa el cambio, el cálculo de DV y el evento de bitácora de usuarios y perfiles en una transacción de aplicación. En una base nueva, el formulario inicial o las variables de entorno aportan las contraseñas para un administrador y dos cuentas Invitado de ejemplo. Las tres altas se confirman juntas; el repositorio no contiene claves iniciales conocidas. La conexión se configura una sola vez en `App.config`, con posibilidad de sustituirla mediante `TPINGSOFTWARE_CONNECTION_STRING` durante pruebas o instalaciones. Las operaciones de respaldo y restauración siguen siendo comandos de instancia en DAL: SQL Server no permite ejecutarlas dentro de una transacción de usuario ni alojar el procedimiento de restauración en la misma base que se está restaurando.

Todavía no hay módulos funcionales de PN1 o PN2 ni tablas de negocio en la base actual.

`MapperBase_33ZS`, `MappingHandler_33ZS` y `ResultadoOperacion_33ZS` están definidos como apoyo para mappers, pero las operaciones existentes todavía no los utilizan. Su presencia no demuestra por sí sola que el recorrido de datos esté alineado.

## Estado de la arquitectura

- **Alineado:** los formularios usan BLL, la BLL usa mappers, los mappers usan accesos DAL por módulo y DAL ejecuta SP. `BE` y `DAL` no dependen de otras capas de la solución.
- **Seguridad existente:** su lógica está distribuida entre `BLL`, `Mappers` y `Servicios`. Se mantiene esa organización para preservar el funcionamiento del scaffold al desarrollar PN1 y PN2.
- **Pendiente:** poblar `BE` con entidades del dominio Harlem y desarrollar los módulos de PN1 y PN2. Las entidades heredadas de seguridad continúan en `Servicios` para preservar la compatibilidad del scaffold.

## Organización objetivo para PN1 y PN2

Cada módulo funcional nuevo tendrá archivos correspondientes en las capas necesarias. El primer recorrido, para identificar o dar de alta un cliente (CUN01), puede organizarse así:

```text
BE/Clientes/Cliente_33ZS.cs
BLL/Clientes/ClienteBLL_33ZS.cs
Mappers/Clientes/ClienteMapper_33ZS.cs
DAL/Clientes/ClienteDataAccess_33ZS.cs
Servicios/Migrations/005_Clientes.sql  (tablas y SP versionados)
TpIngSoftware/Clientes/ClienteForm_33ZS.cs
```

Los módulos siguientes se derivan de los casos de uso documentados: `Atenciones` y `Cobros` para PN1; `Insumos`, `Proveedores` y `Compras` para PN2; `Reportes` para las consultas correspondientes. La carpeta de un módulo se mantiene reconocible entre las capas, sin exigir una clase por cada caso de uso.

Las responsabilidades son:

- `BE`: entidades y tipos del dominio, sin referencias a otras capas.
- `BLL`: validaciones, reglas de negocio y coordinación de operaciones; no contiene SQL ni abre conexiones.
- `Mappers`: transforma resultados de persistencia en entidades y prepara los datos que recibe `DAL`; no administra conexiones ni transacciones.
- `DAL`: invoca procedimientos parametrizados y administra conexiones y transacciones. Los scripts versionados se aplican mediante `Servicios/DatabaseMigrator_33ZS.cs`.
- `Servicios`: mantiene autenticación, autorización, sesión, idiomas, bitácora, respaldo, integridad e infraestructura de migraciones existentes. Su distribución interna puede revisarse sin desplazar ni romper el scaffold actual.
- `TpIngSoftware`: formularios, navegación y presentación; no contiene SQL ni reglas de negocio.

El registro de una atención cobrada (CUN02) debe confirmar conjuntamente la atención, el cobro, la comisión calculada y el descuento fijo de insumos, o no confirmar ninguno. Las tablas nuevas se incorporarán mediante migraciones versionadas; no se vuelve a ejecutar `BD.sql` sobre una base con datos.

Antes de crear tablas o cambiar las existentes, se prueba la migración en una copia restaurada y se respalda la base de trabajo. `BD.sql` y `QueryCargarDatosIniciales.sql` sólo se usan para crear una base nueva; nunca se vuelven a ejecutar sobre datos existentes.
