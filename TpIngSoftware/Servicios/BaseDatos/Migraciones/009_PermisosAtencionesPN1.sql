SET XACT_ABORT ON;
GO

-- Comprueba patentes directas y las heredadas por familias, incluidas las anidadas.
CREATE OR ALTER FUNCTION dbo.Rol_TienePatente_33ZS
(
    @NombreRol nvarchar(100), @NombrePatente nvarchar(100)
)
RETURNS bit
AS
BEGIN
    DECLARE @RolID int, @PatenteID int;
    SELECT @RolID = ID FROM dbo.Rol WHERE Nombre = @NombreRol;
    SELECT @PatenteID = ID FROM dbo.Patente WHERE Nombre = @NombrePatente;
    IF @RolID IS NULL OR @PatenteID IS NULL RETURN 0;

    IF EXISTS (SELECT 1 FROM dbo.Rol_Patente
               WHERE RolID = @RolID AND PatenteID = @PatenteID)
        RETURN 1;

    DECLARE @Familias TABLE (FamiliaID int NOT NULL PRIMARY KEY);
    INSERT INTO @Familias (FamiliaID)
    SELECT FamiliaID FROM dbo.Rol_Familia WHERE RolID = @RolID;

    WHILE 1 = 1
    BEGIN
        INSERT INTO @Familias (FamiliaID)
        SELECT DISTINCT fn.SubFamiliaID
        FROM dbo.FamiliaN fn
        JOIN @Familias f ON f.FamiliaID = fn.FamiliaID
        WHERE NOT EXISTS (SELECT 1 FROM @Familias existente
                          WHERE existente.FamiliaID = fn.SubFamiliaID);
        IF @@ROWCOUNT = 0 BREAK;
    END;

    IF EXISTS (SELECT 1 FROM dbo.Patente_Familia pf
               JOIN @Familias f ON f.FamiliaID = pf.FamiliaID
               WHERE pf.PatenteID = @PatenteID)
        RETURN 1;
    RETURN 0;
END
GO

CREATE OR ALTER PROCEDURE dbo.Atencion_Registrar_33ZS
    @OperacionID uniqueidentifier, @ClienteID int, @BarberoDNI varchar(20),
    @RecepcionistaDNI varchar(20), @ServicioID int,
    @MedioPagoID int, @Importe decimal(12,2)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        DECLARE @Existente int;
        SELECT @Existente = AtencionID FROM dbo.Atencion WITH (UPDLOCK, HOLDLOCK)
        WHERE OperacionID = @OperacionID;
        IF @Existente IS NOT NULL
        BEGIN
            COMMIT;
            SELECT @Existente AS AtencionID;
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM dbo.Cliente WHERE ClienteID = @ClienteID AND Activo = 1)
            THROW 51010, 'Cliente no disponible.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.Usuario u
                       WHERE u.DNI = @RecepcionistaDNI AND u.Activo = 1 AND u.Bloqueo = 0
                         AND dbo.Rol_TienePatente_33ZS(u.Rol, N'RegistrarAtencion') = 1)
            THROW 51011, 'Usuario sin permiso para registrar atenciones.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.Usuario u JOIN dbo.BarberoPerfil b ON b.UsuarioDNI = u.DNI
                       WHERE u.DNI = @BarberoDNI AND u.Rol = N'Barbero'
                         AND u.Activo = 1 AND u.Bloqueo = 0 AND b.Activo = 1)
            THROW 51012, 'Barbero no disponible.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.MedioPago WHERE MedioPagoID = @MedioPagoID AND Activo = 1)
            THROW 51013, 'Medio de pago no disponible.', 1;

        DECLARE @Precio decimal(12,2), @Porcentaje decimal(5,2);
        SELECT @Precio = Precio FROM dbo.ServicioCatalogo WITH (UPDLOCK, HOLDLOCK)
        WHERE ServicioID = @ServicioID AND Activo = 1;
        SELECT @Porcentaje = PorcentajeComision FROM dbo.BarberoPerfil WITH (UPDLOCK, HOLDLOCK)
        WHERE UsuarioDNI = @BarberoDNI AND Activo = 1;
        IF @Precio IS NULL OR @Porcentaje IS NULL
            THROW 51014, 'Precio o comision sin configurar.', 1;
        IF @Importe IS NULL OR @Importe <> @Precio
            THROW 51015, 'El cobro debe coincidir con el precio del servicio.', 1;

        DECLARE @Requeridos int = (SELECT COUNT(*) FROM dbo.ServicioConsumo WHERE ServicioID = @ServicioID);
        IF @Requeridos = 0 THROW 51016, 'Servicio sin consumos configurados.', 1;

        UPDATE i SET Stock = i.Stock - sc.Cantidad
        FROM dbo.Insumo i JOIN dbo.ServicioConsumo sc ON sc.InsumoID = i.InsumoID
        WHERE sc.ServicioID = @ServicioID AND i.Activo = 1 AND i.Stock >= sc.Cantidad;
        IF @@ROWCOUNT <> @Requeridos
            THROW 51017, 'Stock insuficiente o insumo inactivo.', 1;

        DECLARE @Comision decimal(12,2) = ROUND(@Precio * @Porcentaje / 100, 2);
        INSERT INTO dbo.Atencion
            (OperacionID, ClienteID, BarberoDNI, RecepcionistaDNI, ServicioID,
             PrecioAplicado, PorcentajeComisionAplicado, ComisionImporte)
        VALUES
            (@OperacionID, @ClienteID, @BarberoDNI, @RecepcionistaDNI, @ServicioID,
             @Precio, @Porcentaje, @Comision);
        DECLARE @AtencionID int = CONVERT(int, SCOPE_IDENTITY());
        INSERT INTO dbo.Cobro (AtencionID, MedioPagoID, Importe)
        VALUES (@AtencionID, @MedioPagoID, @Importe);
        INSERT INTO dbo.ConsumoAtencion (AtencionID, InsumoID, Cantidad)
        SELECT @AtencionID, InsumoID, Cantidad
        FROM dbo.ServicioConsumo WHERE ServicioID = @ServicioID;

        COMMIT;
        SELECT @AtencionID AS AtencionID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO
