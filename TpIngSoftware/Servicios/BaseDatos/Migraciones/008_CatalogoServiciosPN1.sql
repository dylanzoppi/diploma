CREATE TYPE dbo.ConsumosServicio_33ZS AS TABLE
(
    InsumoID int NOT NULL PRIMARY KEY,
    Cantidad decimal(12,2) NOT NULL
);
GO

CREATE OR ALTER PROCEDURE dbo.Servicio_Guardar_33ZS
    @ServicioID int = NULL,
    @Nombre nvarchar(100),
    @Precio decimal(12,2),
    @Activo bit,
    @Consumos dbo.ConsumosServicio_33ZS READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @Nombre = LTRIM(RTRIM(@Nombre));

    IF @Nombre IS NULL OR @Nombre = N'' OR @Precio IS NULL OR @Precio <= 0
       OR @Activo IS NULL OR (@ServicioID IS NOT NULL AND @ServicioID <= 0)
        THROW 51030, 'Nombre, precio o servicio invalido.', 1;
    IF NOT EXISTS (SELECT 1 FROM @Consumos)
        THROW 51031, 'Seleccione al menos un insumo para el servicio.', 1;
    IF EXISTS (SELECT 1 FROM @Consumos WHERE Cantidad <= 0)
        THROW 51032, 'La cantidad consumida debe ser positiva.', 1;

    BEGIN TRY
        BEGIN TRAN;

        IF EXISTS (
            SELECT 1 FROM @Consumos c
            LEFT JOIN dbo.Insumo i WITH (UPDLOCK, HOLDLOCK) ON i.InsumoID = c.InsumoID
            WHERE i.InsumoID IS NULL OR i.Activo = 0
        )
            THROW 51033, 'Uno de los insumos no existe o esta inactivo.', 1;

        IF @ServicioID IS NULL
        BEGIN
            INSERT INTO dbo.ServicioCatalogo (Nombre, Precio, Activo)
            VALUES (@Nombre, @Precio, @Activo);
            SET @ServicioID = CONVERT(int, SCOPE_IDENTITY());
        END
        ELSE
        BEGIN
            UPDATE dbo.ServicioCatalogo
            SET Nombre = @Nombre, Precio = @Precio, Activo = @Activo
            WHERE ServicioID = @ServicioID;
            IF @@ROWCOUNT = 0 THROW 51034, 'Servicio inexistente.', 1;
            DELETE FROM dbo.ServicioConsumo WHERE ServicioID = @ServicioID;
        END

        INSERT INTO dbo.ServicioConsumo (ServicioID, InsumoID, Cantidad)
        SELECT @ServicioID, InsumoID, Cantidad FROM @Consumos;

        COMMIT;
        SELECT @ServicioID AS ServicioID;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.Servicio_Consumos_33ZS
    @ServicioID int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.InsumoID, i.Codigo, i.Nombre, sc.Cantidad, i.Stock, i.Activo
    FROM dbo.ServicioConsumo sc
    JOIN dbo.Insumo i ON i.InsumoID = sc.InsumoID
    WHERE sc.ServicioID = @ServicioID
    ORDER BY i.Nombre;
END;
GO
