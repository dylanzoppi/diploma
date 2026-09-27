-- Roles y patentes operativos de PN1. El migrador ejecuta este archivo
-- dentro de una transaccion y registra la version solo si termina bien.
SET XACT_ABORT ON;

DECLARE @Roles TABLE (Nombre nvarchar(100) PRIMARY KEY);
INSERT INTO @Roles VALUES (N'Dueño'), (N'Recepcionista'), (N'Barbero');

DECLARE @NombreRol nvarchar(100);
DECLARE roles_cursor CURSOR LOCAL FAST_FORWARD FOR SELECT Nombre FROM @Roles;
OPEN roles_cursor;
FETCH NEXT FROM roles_cursor INTO @NombreRol;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE Nombre = @NombreRol)
        INSERT INTO dbo.Rol (ID, Nombre)
        SELECT ISNULL(MAX(ID), 0) + 1, @NombreRol FROM dbo.Rol WITH (UPDLOCK, HOLDLOCK);
    FETCH NEXT FROM roles_cursor INTO @NombreRol;
END
CLOSE roles_cursor;
DEALLOCATE roles_cursor;

DECLARE @Patentes TABLE (Nombre nvarchar(100) PRIMARY KEY);
INSERT INTO @Patentes VALUES
    (N'BuscarCliente'), (N'RegistrarCliente'),
    (N'ConsultarCatalogo'), (N'GestionarCatalogo'),
    (N'GestionarBarberos'), (N'GestionarStock'),
    (N'RegistrarAtencion'), (N'ConsultarAtencionesPropias'),
    (N'ConsultarAtencionesGenerales');

DECLARE @NombrePatente nvarchar(100);
DECLARE patentes_cursor CURSOR LOCAL FAST_FORWARD FOR SELECT Nombre FROM @Patentes;
OPEN patentes_cursor;
FETCH NEXT FROM patentes_cursor INTO @NombrePatente;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.Patente WHERE Nombre = @NombrePatente)
        INSERT INTO dbo.Patente (ID, Nombre)
        SELECT ISNULL(MAX(ID), 0) + 1, @NombrePatente FROM dbo.Patente WITH (UPDLOCK, HOLDLOCK);
    FETCH NEXT FROM patentes_cursor INTO @NombrePatente;
END
CLOSE patentes_cursor;
DEALLOCATE patentes_cursor;

-- El administrador existente conserva acceso al scaffold y recibe PN1.
INSERT INTO dbo.Rol_Patente (RolID, PatenteID)
SELECT r.ID, p.ID
FROM dbo.Rol r CROSS JOIN dbo.Patente p
WHERE r.Nombre IN (N'Administrador', N'Dueño')
  AND (r.Nombre = N'Administrador'
       OR p.Nombre IN (N'BuscarCliente', N'RegistrarCliente', N'ConsultarCatalogo',
                       N'GestionarCatalogo', N'GestionarBarberos', N'GestionarStock',
                       N'RegistrarAtencion', N'ConsultarAtencionesGenerales',
                       N'AltaUsuario', N'BajaUsuario', N'ModificacionUsuario',
                       N'DesbloquearUsuario', N'CambiarClave'))
  AND NOT EXISTS (SELECT 1 FROM dbo.Rol_Patente rp WHERE rp.RolID = r.ID AND rp.PatenteID = p.ID);

INSERT INTO dbo.Rol_Patente (RolID, PatenteID)
SELECT r.ID, p.ID
FROM dbo.Rol r JOIN dbo.Patente p ON p.Nombre IN
    (N'BuscarCliente', N'RegistrarCliente', N'ConsultarCatalogo',
     N'RegistrarAtencion', N'CambiarClave')
WHERE r.Nombre = N'Recepcionista'
  AND NOT EXISTS (SELECT 1 FROM dbo.Rol_Patente rp WHERE rp.RolID = r.ID AND rp.PatenteID = p.ID);

INSERT INTO dbo.Rol_Patente (RolID, PatenteID)
SELECT r.ID, p.ID
FROM dbo.Rol r JOIN dbo.Patente p ON p.Nombre IN
    (N'ConsultarAtencionesPropias', N'CambiarClave')
WHERE r.Nombre = N'Barbero'
  AND NOT EXISTS (SELECT 1 FROM dbo.Rol_Patente rp WHERE rp.RolID = r.ID AND rp.PatenteID = p.ID);

-- El alta de roles y patentes altera tablas protegidas por DV. Esta marca
-- permite completar el recálculo una sola vez y recuperarlo tras un cierre abrupto.
IF OBJECT_ID(N'dbo.PN1EstadoMigracion', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PN1EstadoMigracion
    (
        Id tinyint NOT NULL CONSTRAINT PK_PN1EstadoMigracion PRIMARY KEY,
        DVActualizado bit NOT NULL,
        CONSTRAINT CK_PN1EstadoMigracion_Id CHECK (Id = 1)
    );
    INSERT INTO dbo.PN1EstadoMigracion (Id, DVActualizado) VALUES (1, 0);
END;
GO

CREATE OR ALTER PROCEDURE dbo.PN1_DVPendiente_33ZS
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DVActualizado FROM dbo.PN1EstadoMigracion WHERE Id = 1;
END
GO

CREATE OR ALTER PROCEDURE dbo.PN1_ConfirmarDV_33ZS
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    UPDATE dbo.PN1EstadoMigracion SET DVActualizado = 1 WHERE Id = 1;
    IF @@ROWCOUNT <> 1 THROW 51030, 'Falta el estado de migracion PN1.', 1;
END
