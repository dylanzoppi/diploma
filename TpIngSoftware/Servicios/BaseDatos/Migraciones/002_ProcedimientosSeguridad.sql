-- La migración completa se ejecuta dentro de la transacción de SchemaMigration.
-- Cada operación de escritura tiene su propia transacción y propaga el error.
IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='UQ_Familia_Nombre'
    AND parent_object_id=OBJECT_ID(N'dbo.Familia'))
BEGIN
    IF EXISTS (SELECT Nombre FROM dbo.Familia GROUP BY Nombre HAVING COUNT(*)>1)
        THROW 52008, 'Existen familias con nombre duplicado.', 1;
    ALTER TABLE dbo.Familia ADD CONSTRAINT UQ_Familia_Nombre UNIQUE (Nombre);
END;
IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='UQ_Usuario_Login'
    AND parent_object_id=OBJECT_ID(N'dbo.Usuario'))
BEGIN
    IF EXISTS (SELECT Login FROM dbo.Usuario GROUP BY Login HAVING COUNT(*)>1)
        THROW 52009, 'Existen usuarios con login duplicado.', 1;
    ALTER TABLE dbo.Usuario ADD CONSTRAINT UQ_Usuario_Login UNIQUE (Login);
END;

IF TYPE_ID(N'dbo.ComponentePerfil_33ZS') IS NULL
    EXEC(N'CREATE TYPE dbo.ComponentePerfil_33ZS AS TABLE (Id int NOT NULL, Tipo char(1) NOT NULL, PRIMARY KEY (Tipo, Id))');
GO

CREATE OR ALTER PROCEDURE dbo.Usuario_ObtenerTodos_33ZS AS
BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Usuario ORDER BY DNI; END;
GO
CREATE OR ALTER PROCEDURE dbo.Usuario_ObtenerPorLogin_33ZS @Login varchar(150) AS
BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Usuario WHERE Login=@Login OR Email=@Login; END;
GO
CREATE OR ALTER PROCEDURE dbo.Usuario_ObtenerPorEstado_33ZS @Activo bit AS
BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Usuario WHERE Activo=@Activo ORDER BY DNI; END;
GO
CREATE OR ALTER PROCEDURE dbo.Usuario_ObtenerPorNombre_33ZS @Login varchar(150) AS
BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Usuario WHERE Login=@Login; END;
GO
CREATE OR ALTER PROCEDURE dbo.Usuario_ObtenerPorDNI_33ZS @DNI varchar(20) AS
BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Usuario WHERE DNI=@DNI; END;
GO
CREATE OR ALTER PROCEDURE dbo.Usuario_Agregar_33ZS
    @DNI varchar(20), @Apellidos varchar(100), @Nombre varchar(100),
    @Login varchar(150), @Password varchar(255), @Rol nvarchar(100),
    @Email varchar(150), @Bloqueo bit, @Activo bit
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        IF EXISTS(SELECT 1 FROM dbo.Usuario WHERE Login IN (@Login,@Email) OR Email IN (@Login,@Email))
            THROW 52010, 'Login o email ya asignado.', 1;
        INSERT INTO dbo.Usuario(DNI,Apellidos,Nombre,Login,Password,Rol,Email,Bloqueo,Activo)
        VALUES(@DNI,@Apellidos,@Nombre,@Login,@Password,@Rol,@Email,@Bloqueo,@Activo);
        COMMIT;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        THROW;
    END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Usuario_Modificar_33ZS
    @DNI varchar(20), @Apellidos varchar(100), @Nombre varchar(100),
    @Login varchar(150), @Rol nvarchar(100), @Email varchar(150),
    @Bloqueo bit, @Activo bit
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        IF EXISTS(SELECT 1 FROM dbo.Usuario WHERE DNI<>@DNI
                  AND (Login IN (@Login,@Email) OR Email IN (@Login,@Email)))
            THROW 52010, 'Login o email ya asignado.', 1;
        UPDATE dbo.Usuario SET Apellidos=@Apellidos,Nombre=@Nombre,Login=@Login,
            Rol=@Rol,Email=@Email,Bloqueo=@Bloqueo,Activo=@Activo WHERE DNI=@DNI;
        IF @@ROWCOUNT <> 1 THROW 52001, 'Usuario inexistente.', 1;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        THROW;
    END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Usuario_Bloquear_33ZS @DNI varchar(20) AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE dbo.Usuario SET Bloqueo=1 WHERE DNI=@DNI;
        IF @@ROWCOUNT <> 1 THROW 52001, 'Usuario inexistente.', 1;
        COMMIT;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Usuario_Desbloquear_33ZS @DNI varchar(20) AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE dbo.Usuario SET Bloqueo=0 WHERE DNI=@DNI;
        IF @@ROWCOUNT <> 1 THROW 52001, 'Usuario inexistente.', 1;
        COMMIT;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Usuario_CambiarEstado_33ZS @DNI varchar(20), @Activo bit AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE dbo.Usuario SET Activo=@Activo WHERE DNI=@DNI;
        IF @@ROWCOUNT <> 1 THROW 52001, 'Usuario inexistente.', 1;
        COMMIT;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Usuario_ActualizarIdioma_33ZS @DNI varchar(20), @Idioma varchar(10) AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE dbo.Usuario SET Idioma=@Idioma WHERE DNI=@DNI;
        IF @@ROWCOUNT <> 1 THROW 52001, 'Usuario inexistente.', 1;
        COMMIT;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Usuario_CambiarClave_33ZS @DNI varchar(20), @Password varchar(255) AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE dbo.Usuario SET Password=@Password WHERE DNI=@DNI;
        IF @@ROWCOUNT <> 1 THROW 52001, 'Usuario inexistente.', 1;
        COMMIT;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.Perfil_ObtenerPatentes_33ZS AS
BEGIN SET NOCOUNT ON; SELECT ID,Nombre FROM dbo.Patente ORDER BY ID; END;
GO
CREATE OR ALTER PROCEDURE dbo.Perfil_ObtenerFamilias_33ZS AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID,Nombre FROM dbo.Patente ORDER BY ID;
    SELECT ID,Nombre FROM dbo.Familia ORDER BY ID;
    SELECT PatenteID,FamiliaID FROM dbo.Patente_Familia ORDER BY PatenteID,FamiliaID;
    SELECT FamiliaID,SubFamiliaID FROM dbo.FamiliaN ORDER BY FamiliaID,SubFamiliaID;
END;
GO
CREATE OR ALTER PROCEDURE dbo.Perfil_ObtenerRoles_33ZS AS
BEGIN
    SET NOCOUNT ON;
    EXEC dbo.Perfil_ObtenerFamilias_33ZS;
    SELECT ID,Nombre FROM dbo.Rol ORDER BY ID;
    SELECT RolID,PatenteID FROM dbo.Rol_Patente ORDER BY RolID,PatenteID;
    SELECT RolID,FamiliaID FROM dbo.Rol_Familia ORDER BY RolID,FamiliaID;
END;
GO
CREATE OR ALTER PROCEDURE dbo.Perfil_GuardarFamilia_33ZS
    @Nombre nvarchar(100), @Componentes dbo.ComponentePerfil_33ZS READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF EXISTS(SELECT 1 FROM @Componentes WHERE Tipo NOT IN ('P','F'))
        THROW 52002, 'Tipo de componente inválido.', 1;
    DECLARE @Id int;
    BEGIN TRY
        SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
        BEGIN TRAN;
        SELECT @Id=ISNULL(MAX(ID),0)+1 FROM dbo.Familia WITH (UPDLOCK,HOLDLOCK);
        IF EXISTS(SELECT 1 FROM @Componentes WHERE Tipo='F' AND Id=@Id)
            THROW 52011, 'La familia no puede contenerse a sí misma.', 1;
        INSERT INTO dbo.Familia(ID,Nombre) VALUES(@Id,@Nombre);
        INSERT INTO dbo.Patente_Familia(PatenteID,FamiliaID)
            SELECT Id,@Id FROM @Componentes WHERE Tipo='P';
        INSERT INTO dbo.FamiliaN(FamiliaID,SubFamiliaID)
            SELECT @Id,Id FROM @Componentes WHERE Tipo='F';
        COMMIT;
        SELECT @Id;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Perfil_ModificarFamilia_33ZS
    @Id int, @Nombre nvarchar(100), @Componentes dbo.ComponentePerfil_33ZS READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF EXISTS(SELECT 1 FROM @Componentes WHERE Tipo NOT IN ('P','F') OR (Tipo='F' AND Id=@Id))
        THROW 52002, 'Tipo o referencia de componente inválida.', 1;
    DECLARE @TieneCiclo bit=0;
    ;WITH Descendientes AS
    (
        SELECT Id FROM @Componentes WHERE Tipo='F'
        UNION ALL
        SELECT fn.SubFamiliaID FROM dbo.FamiliaN fn
        JOIN Descendientes d ON d.Id=fn.FamiliaID
    )
    SELECT @TieneCiclo=CASE WHEN EXISTS(SELECT 1 FROM Descendientes WHERE Id=@Id)
        THEN 1 ELSE 0 END OPTION (MAXRECURSION 32767);
    IF @TieneCiclo=1 THROW 52011, 'La relación crearía un ciclo de familias.', 1;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE dbo.Familia SET Nombre=@Nombre WHERE ID=@Id;
        IF @@ROWCOUNT <> 1 THROW 52003, 'Familia inexistente.', 1;
        DELETE FROM dbo.Patente_Familia WHERE FamiliaID=@Id;
        DELETE FROM dbo.FamiliaN WHERE FamiliaID=@Id;
        INSERT INTO dbo.Patente_Familia(PatenteID,FamiliaID)
            SELECT Id,@Id FROM @Componentes WHERE Tipo='P';
        INSERT INTO dbo.FamiliaN(FamiliaID,SubFamiliaID)
            SELECT @Id,Id FROM @Componentes WHERE Tipo='F';
        COMMIT;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Perfil_EliminarFamilia_33ZS @Id int AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        IF EXISTS(SELECT 1 FROM dbo.Rol_Familia rf JOIN dbo.Rol r ON r.ID=rf.RolID
                  JOIN dbo.Usuario u ON u.Rol=r.Nombre WHERE rf.FamiliaID=@Id)
            THROW 52004, 'La familia está asignada a un rol en uso.', 1;
        DELETE FROM dbo.Patente_Familia WHERE FamiliaID=@Id;
        DELETE FROM dbo.FamiliaN WHERE FamiliaID=@Id OR SubFamiliaID=@Id;
        DELETE FROM dbo.Rol_Familia WHERE FamiliaID=@Id;
        DELETE FROM dbo.Familia WHERE ID=@Id;
        IF @@ROWCOUNT <> 1 THROW 52003, 'Familia inexistente.', 1;
        COMMIT;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Perfil_FamiliaEnUso_33ZS @Id int AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) FROM dbo.Rol_Familia rf JOIN dbo.Rol r ON r.ID=rf.RolID
        JOIN dbo.Usuario u ON u.Rol=r.Nombre WHERE rf.FamiliaID=@Id;
END;
GO
CREATE OR ALTER PROCEDURE dbo.Perfil_GuardarRol_33ZS
    @Nombre nvarchar(100), @Componentes dbo.ComponentePerfil_33ZS READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF EXISTS(SELECT 1 FROM @Componentes WHERE Tipo NOT IN ('P','F'))
        THROW 52002, 'Tipo de componente inválido.', 1;
    DECLARE @Id int;
    BEGIN TRY
        SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
        BEGIN TRAN;
        SELECT @Id=ISNULL(MAX(ID),0)+1 FROM dbo.Rol WITH (UPDLOCK,HOLDLOCK);
        INSERT INTO dbo.Rol(ID,Nombre) VALUES(@Id,@Nombre);
        INSERT INTO dbo.Rol_Patente(RolID,PatenteID)
            SELECT @Id,Id FROM @Componentes WHERE Tipo='P';
        INSERT INTO dbo.Rol_Familia(RolID,FamiliaID)
            SELECT @Id,Id FROM @Componentes WHERE Tipo='F';
        COMMIT;
        SELECT @Id;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Perfil_ModificarRol_33ZS
    @Id int, @Nombre nvarchar(100), @Componentes dbo.ComponentePerfil_33ZS READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF EXISTS(SELECT 1 FROM @Componentes WHERE Tipo NOT IN ('P','F'))
        THROW 52002, 'Tipo de componente inválido.', 1;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE dbo.Rol SET Nombre=@Nombre WHERE ID=@Id;
        IF @@ROWCOUNT <> 1 THROW 52005, 'Rol inexistente.', 1;
        DELETE FROM dbo.Rol_Patente WHERE RolID=@Id;
        DELETE FROM dbo.Rol_Familia WHERE RolID=@Id;
        INSERT INTO dbo.Rol_Patente(RolID,PatenteID)
            SELECT @Id,Id FROM @Componentes WHERE Tipo='P';
        INSERT INTO dbo.Rol_Familia(RolID,FamiliaID)
            SELECT @Id,Id FROM @Componentes WHERE Tipo='F';
        COMMIT;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Perfil_EliminarRol_33ZS @Id int AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        IF EXISTS(SELECT 1 FROM dbo.Usuario u JOIN dbo.Rol r ON r.Nombre=u.Rol WHERE r.ID=@Id)
            THROW 52006, 'El rol está asignado a usuarios.', 1;
        DELETE FROM dbo.Rol_Patente WHERE RolID=@Id;
        DELETE FROM dbo.Rol_Familia WHERE RolID=@Id;
        DELETE FROM dbo.Rol WHERE ID=@Id;
        IF @@ROWCOUNT <> 1 THROW 52005, 'Rol inexistente.', 1;
        COMMIT;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Perfil_RolEnUso_33ZS @Id int AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) FROM dbo.Usuario u JOIN dbo.Rol r ON r.Nombre=u.Rol WHERE r.ID=@Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.Evento_Registrar_33ZS
    @Login varchar(150), @Fecha datetime, @Hora datetime, @Modulo varchar(50),
    @EventoNombre varchar(100), @Criticidad int
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    DECLARE @Id int;
    BEGIN TRY
        SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
        BEGIN TRAN;
        SELECT @Id=ISNULL(MAX(Id_Evento),0)+1 FROM dbo.Evento WITH (UPDLOCK,HOLDLOCK);
        INSERT INTO dbo.Evento(Id_Evento,Login,Fecha,Hora,Modulo,Evento,Criticidad)
            VALUES(@Id,@Login,@Fecha,@Hora,@Modulo,@EventoNombre,@Criticidad);
        COMMIT;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.Evento_ObtenerTodos_33ZS AS
BEGIN SET NOCOUNT ON; SELECT * FROM dbo.Evento ORDER BY Fecha DESC,Hora DESC; END;
GO

CREATE OR ALTER PROCEDURE dbo.DV_ObtenerTabla_33ZS @Tabla varchar(50) AS
BEGIN
    SET NOCOUNT ON;
    IF @Tabla='Usuario' SELECT * FROM dbo.Usuario ORDER BY DNI;
    ELSE IF @Tabla='Rol' SELECT * FROM dbo.Rol ORDER BY ID;
    ELSE IF @Tabla='Patente' SELECT * FROM dbo.Patente ORDER BY ID;
    ELSE IF @Tabla='Familia' SELECT * FROM dbo.Familia ORDER BY ID;
    ELSE IF @Tabla='Rol_Patente' SELECT * FROM dbo.Rol_Patente ORDER BY RolID,PatenteID;
    ELSE IF @Tabla='Rol_Familia' SELECT * FROM dbo.Rol_Familia ORDER BY RolID,FamiliaID;
    ELSE IF @Tabla='Patente_Familia' SELECT * FROM dbo.Patente_Familia ORDER BY PatenteID,FamiliaID;
    ELSE IF @Tabla='FamiliaN' SELECT * FROM dbo.FamiliaN ORDER BY FamiliaID,SubFamiliaID;
    ELSE THROW 52007, 'Tabla de integridad no permitida.', 1;
END;
GO
CREATE OR ALTER PROCEDURE dbo.DV_ObtenerGuardado_33ZS @Tabla varchar(50) AS
BEGIN SET NOCOUNT ON; SELECT DVH,DVV FROM dbo.DV WHERE NombreTabla=@Tabla; END;
GO
CREATE OR ALTER PROCEDURE dbo.DV_Guardar_33ZS
    @Tabla varchar(50), @DVH varchar(64), @DVV varchar(64)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF @Tabla NOT IN ('Usuario','Rol','Patente','Familia','Rol_Patente','Rol_Familia','Patente_Familia','FamiliaN')
        THROW 52007, 'Tabla de integridad no permitida.', 1;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE dbo.DV WITH (UPDLOCK,SERIALIZABLE) SET DVH=@DVH,DVV=@DVV WHERE NombreTabla=@Tabla;
        IF @@ROWCOUNT=0 INSERT INTO dbo.DV(NombreTabla,DVH,DVV) VALUES(@Tabla,@DVH,@DVV);
        COMMIT;
    END TRY
    BEGIN CATCH IF XACT_STATE() <> 0 ROLLBACK; THROW; END CATCH
END;
GO
