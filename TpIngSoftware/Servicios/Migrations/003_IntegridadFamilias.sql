-- No se alteran datos existentes. Las referencias se comprueban en el SP antes de borrar.
CREATE OR ALTER PROCEDURE dbo.Perfil_FamiliaEnUso_33ZS @Id int AS
BEGIN
    SET NOCOUNT ON;
    SELECT (SELECT COUNT(*) FROM dbo.FamiliaN WHERE SubFamiliaID=@Id)
         + (SELECT COUNT(*) FROM dbo.Rol_Familia WHERE FamiliaID=@Id);
END;
GO

CREATE OR ALTER PROCEDURE dbo.Perfil_EliminarFamilia_33ZS @Id int AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRY
        SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
        BEGIN TRAN;
        IF EXISTS(SELECT 1 FROM dbo.FamiliaN WHERE SubFamiliaID=@Id)
           OR EXISTS(SELECT 1 FROM dbo.Rol_Familia WHERE FamiliaID=@Id)
            THROW 52004, 'La familia está referenciada por otra familia o un rol.', 1;
        DELETE FROM dbo.Patente_Familia WHERE FamiliaID=@Id;
        DELETE FROM dbo.FamiliaN WHERE FamiliaID=@Id;
        DELETE FROM dbo.Familia WHERE ID=@Id;
        IF @@ROWCOUNT <> 1 THROW 52003, 'Familia inexistente.', 1;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.Perfil_ModificarFamilia_33ZS
    @Id int, @Nombre nvarchar(100), @Componentes dbo.ComponentePerfil_33ZS READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF EXISTS(SELECT 1 FROM @Componentes WHERE Tipo NOT IN ('P','F') OR (Tipo='F' AND Id=@Id))
        THROW 52002, 'Tipo o referencia de componente inválida.', 1;
    BEGIN TRY
        SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
        BEGIN TRAN;
        IF NOT EXISTS(SELECT 1 FROM dbo.Familia WHERE ID=@Id)
            THROW 52003, 'Familia inexistente.', 1;
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
        UPDATE dbo.Familia SET Nombre=@Nombre WHERE ID=@Id;
        DELETE FROM dbo.Patente_Familia WHERE FamiliaID=@Id;
        DELETE FROM dbo.FamiliaN WHERE FamiliaID=@Id;
        INSERT INTO dbo.Patente_Familia(PatenteID,FamiliaID)
            SELECT Id,@Id FROM @Componentes WHERE Tipo='P';
        INSERT INTO dbo.FamiliaN(FamiliaID,SubFamiliaID)
            SELECT @Id,Id FROM @Componentes WHERE Tipo='F';
        COMMIT;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        THROW;
    END CATCH
END;
GO
