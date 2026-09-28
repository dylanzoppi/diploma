# Arquitectura de Harlem

## Referencias y alcance

La documentación funcional del Trabajo Final define el alcance de Harlem. La barbería atiende a clientes que se presentan en el local: la recepcionista los identifica o registra allí y, después de prestar el servicio, registra la atención y el cobro. Los procesos son PN1 (prestación y cobro de servicios) y PN2 (compra y reposición de insumos). No se implementan turnos, reservas ni agenda de disponibilidad.

La solución separa presentación, reglas de negocio, transformación de datos y persistencia. Cada módulo nuevo debe mantener reconocible su recorrido entre esas capas y versionar los cambios de esquema junto con los procedimientos almacenados.

## Estado actual de la solución Harlem

La solución usa `TpIngSoftware` como proyecto de interfaz, `Servicios` para seguridad y servicios compartidos, y proyectos separados `BE`, `BLL`, `Mappers` y `DAL`. Sus referencias actuales son:

```text
TpIngSoftware -> BE, BLL, Servicios
BLL           -> BE, Mappers, Servicios
Mappers       -> BE, DAL, Servicios
Servicios     -> BE, DAL
DAL           -> sin referencias a otros proyectos de la solución
BE            -> sin referencias a otros proyectos de la solución
```

El scaffold implementa usuarios, roles y permisos, sesión, idiomas, bitácora, respaldo e integridad. `BLL` coordina las operaciones y los dígitos verificadores; los mappers convierten entidades y conjuntos de resultados; `DAL` contiene clases de acceso por módulo que invocan procedimientos almacenados a través de `DAL/Persistence/Acceso_33ZS.cs`. `BE/PN1` contiene las entidades de negocio; algunas entidades heredadas de seguridad están en `Servicios`.

Las contraseñas nuevas usan PBKDF2 con sal individual. El inicio de sesión reconoce los hashes SHA-256 heredados y los reemplaza tras una validación correcta; `Encriptador_33ZS.Hash` sigue usándose para el cálculo de dígitos verificadores. Los parámetros de política ajustables están en `App.config`.

Las migraciones y la inicialización de la base permanecen en `Servicios/BaseDatos`. Las migraciones `002` a `004` cubren procedimientos de seguridad e intentos de acceso. `005` crea los puestos y patentes de PN1; `006` crea clientes, barberos, catálogo, stock y medios de pago; `007` crea las atenciones y cobros. Las escrituras usan transacción, `XACT_ABORT`, rollback y propagación de errores. La BLL agrupa el cambio, el cálculo de DV y el evento de bitácora de usuarios y perfiles en una transacción de aplicación. En una base nueva, el formulario inicial o las variables de entorno aportan las contraseñas para un administrador, una recepcionista y un barbero de ejemplo. Las tres altas se confirman juntas; el repositorio no contiene claves iniciales conocidas. La conexión se configura en `App.config`, con posibilidad de sustituirla mediante `TPINGSOFTWARE_CONNECTION_STRING` durante pruebas. Las operaciones de respaldo y restauración siguen siendo comandos de instancia en DAL.

La migración `008` agrega el guardado transaccional de servicios y cantidades fijas de insumos. La disponibilidad previa se evalúa en BLL con datos del catálogo, y el SP de atención vuelve a comprobar el stock dentro de la transacción. El reporte entrega cantidad, ingresos y comisiones desde el SP mediante mapper y BLL, sin calcular los totales en la pantalla.

PN1 ya tiene flujo funcional presencial y tablas propias. PN2 (compras y reposición de insumos) todavía no está implementado.

`MapperBase_33ZS`, `MappingHandler_33ZS` y `ResultadoOperacion_33ZS` están definidos como apoyo para mappers, pero las operaciones existentes todavía no los utilizan. Su presencia no demuestra por sí sola que el recorrido de datos esté alineado.

## Estado de la arquitectura

- **Alineado:** los formularios usan BLL, la BLL usa mappers, los mappers usan accesos DAL por módulo y DAL ejecuta SP. `BE` y `DAL` no dependen de otras capas de la solución.
- **Seguridad existente:** su lógica está distribuida entre `BLL`, `Mappers` y `Servicios`. Se mantiene esa organización para preservar el funcionamiento del scaffold al desarrollar PN1 y PN2.
- **Pendiente:** PN2 y sus consultas específicas. Las entidades heredadas de seguridad continúan en `Servicios` para preservar la compatibilidad del scaffold.

## Carpetas del scaffold actual

La interfaz agrupa `Acceso`, `Principal`, `Administracion` (usuarios, perfiles y bitácora) y `Mantenimiento` (integridad y respaldos). En `BLL`, las clases existentes se agrupan por responsabilidad en `Usuarios`, `Perfiles`, `Bitacora`, `Integridad` y `Respaldos`. En `Servicios`, `BaseDatos` contiene el inicializador, el migrador, los scripts de instalación y `Migraciones`; `Seguridad`, `Entidades` y `Observador` reúnen los servicios compartidos. `App.config`, `Program.cs` y los archivos propios de cada proyecto permanecen en su raíz.

Los formularios heredados conservan juntos sus archivos `.cs`, `.Designer.cs` y `.resx`. `Form1` vuelve a tener `Form1.Designer.cs` para editar su estructura en Visual Studio; los accesos según patente se cargan sólo durante la ejecución. Las pantallas de PN1 construyen sus controles en código. La reorganización no cambia las responsabilidades entre capas.

## Organización de PN1 y continuación de PN2

PN1 agrupa sus archivos por proceso en cada capa:

```text
BE/PN1/ModelosPN1_33ZS.cs
BLL/PN1/PN1BLL_33ZS.cs
Mappers/PN1/PN1Mapper_33ZS.cs
DAL/PN1/PN1DataAccess_33ZS.cs
Servicios/BaseDatos/Migraciones/005_RolesPN1.sql ... 008_CatalogoServiciosPN1.sql
TpIngSoftware/PN1/*Form_33ZS.cs
```

PN2 podrá incorporar proveedores, compras y reposición en las mismas capas. Las consultas analíticas y los reportes finales se diseñarán con sus casos de uso, sobre los datos registrados por PN1 y PN2.

Las responsabilidades son:

- `BE`: entidades y tipos del dominio, sin referencias a otras capas.
- `BLL`: validaciones, reglas de negocio y coordinación de operaciones; no contiene SQL ni abre conexiones.
- `Mappers`: transforma resultados de persistencia en entidades y prepara los datos que recibe `DAL`; no administra conexiones ni transacciones.
- `DAL`: invoca procedimientos parametrizados y administra conexiones y transacciones. Los scripts versionados se aplican mediante `Servicios/BaseDatos/DatabaseMigrator_33ZS.cs`.
- `Servicios`: mantiene autenticación, autorización, sesión, idiomas, bitácora, respaldo, integridad e infraestructura de migraciones existentes. Su distribución interna puede revisarse sin desplazar ni romper el scaffold actual.
- `TpIngSoftware`: formularios, navegación y presentación; no contiene SQL ni reglas de negocio.

El registro de una atención cobrada confirma conjuntamente la atención, el cobro, la comisión calculada y el descuento fijo de insumos, o no confirma ninguno. Un `OperacionID` permite reintentar sin duplicar la atención. El barbero consulta su historial mediante BLL usando siempre el DNI de su sesión; la pantalla no recibe un DNI ajeno. Las tablas nuevas se incorporan mediante migraciones versionadas; no se vuelve a ejecutar `BD.sql` sobre una base con datos.

Antes de crear tablas o cambiar las existentes, se prueba la migración en una copia restaurada y se respalda la base de trabajo. `BD.sql` y `QueryCargarDatosIniciales.sql` sólo se usan para crear una base nueva; nunca se vuelven a ejecutar sobre datos existentes.
