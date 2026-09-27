IF OBJECT_ID(N'dbo.LoginIntento', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.LoginIntento
    (
        DNI varchar(20) NOT NULL CONSTRAINT PK_LoginIntento PRIMARY KEY,
        Intentos int NOT NULL CONSTRAINT DF_LoginIntento_Intentos DEFAULT (0),
        CONSTRAINT FK_LoginIntento_Usuario FOREIGN KEY (DNI) REFERENCES dbo.Usuario(DNI),
        CONSTRAINT CK_LoginIntento_Intentos CHECK (Intentos >= 0)
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.Usuario_RegistrarIntentoFallido_33ZS
    @DNI varchar(20), @MaxIntentos int
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF @MaxIntentos < 1 OR @MaxIntentos > 20
        THROW 52013, 'Límite de intentos inválido.', 1;

    BEGIN TRY
        BEGIN TRAN;
        DECLARE @Activo bit, @Bloqueo bit, @Intentos int;
        SELECT @Activo=Activo, @Bloqueo=Bloqueo
        FROM dbo.Usuario WITH (UPDLOCK, HOLDLOCK) WHERE DNI=@DNI;
        IF @Activo IS NULL THROW 52001, 'Usuario inexistente.', 1;
        IF @Activo=0 THROW 52014, 'Usuario inactivo.', 1;
        IF @Bloqueo=1 THROW 52012, 'Usuario bloqueado.', 1;

        UPDATE dbo.LoginIntento SET Intentos=Intentos+1 WHERE DNI=@DNI;
        IF @@ROWCOUNT=0
            INSERT INTO dbo.LoginIntento(DNI,Intentos) VALUES(@DNI,1);

        SELECT @Intentos=Intentos FROM dbo.LoginIntento WHERE DNI=@DNI;
        IF @Intentos >= @MaxIntentos
            UPDATE dbo.Usuario SET Bloqueo=1 WHERE DNI=@DNI;

        COMMIT;
        SELECT @Intentos AS Intentos;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.Usuario_ReiniciarIntentos_33ZS @DNI varchar(20)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        DECLARE @Activo bit, @Bloqueo bit;
        SELECT @Activo=Activo, @Bloqueo=Bloqueo
        FROM dbo.Usuario WITH (UPDLOCK, HOLDLOCK) WHERE DNI=@DNI;
        IF @Activo IS NULL THROW 52001, 'Usuario inexistente.', 1;
        IF @Activo=0 THROW 52014, 'Usuario inactivo.', 1;
        IF @Bloqueo=1 THROW 52012, 'Usuario bloqueado.', 1;
        DELETE FROM dbo.LoginIntento WHERE DNI=@DNI;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.Usuario_Desbloquear_33ZS @DNI varchar(20)
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE dbo.Usuario SET Bloqueo=0 WHERE DNI=@DNI;
        IF @@ROWCOUNT <> 1 THROW 52001, 'Usuario inexistente.', 1;
        DELETE FROM dbo.LoginIntento WHERE DNI=@DNI;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        THROW;
    END CATCH
END;
GO
