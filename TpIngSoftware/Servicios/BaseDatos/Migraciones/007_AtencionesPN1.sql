SET XACT_ABORT ON;

CREATE TABLE dbo.Atencion
(
    AtencionID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Atencion PRIMARY KEY,
    OperacionID uniqueidentifier NOT NULL CONSTRAINT UQ_Atencion_Operacion UNIQUE,
    FechaHora datetime2(0) NOT NULL CONSTRAINT DF_Atencion_Fecha DEFAULT (SYSDATETIME()),
    ClienteID int NOT NULL,
    BarberoDNI varchar(20) NOT NULL,
    RecepcionistaDNI varchar(20) NOT NULL,
    ServicioID int NOT NULL,
    PrecioAplicado decimal(12,2) NOT NULL,
    PorcentajeComisionAplicado decimal(5,2) NOT NULL,
    ComisionImporte decimal(12,2) NOT NULL,
    CONSTRAINT FK_Atencion_Cliente FOREIGN KEY (ClienteID) REFERENCES dbo.Cliente(ClienteID),
    CONSTRAINT FK_Atencion_Barbero FOREIGN KEY (BarberoDNI) REFERENCES dbo.BarberoPerfil(UsuarioDNI),
    CONSTRAINT FK_Atencion_Recepcionista FOREIGN KEY (RecepcionistaDNI) REFERENCES dbo.Usuario(DNI),
    CONSTRAINT FK_Atencion_Servicio FOREIGN KEY (ServicioID) REFERENCES dbo.ServicioCatalogo(ServicioID),
    CONSTRAINT CK_Atencion_Importes CHECK
        (PrecioAplicado > 0 AND PorcentajeComisionAplicado >= 0
         AND PorcentajeComisionAplicado <= 100 AND ComisionImporte >= 0)
);
CREATE INDEX IX_Atencion_BarberoFecha ON dbo.Atencion (BarberoDNI, FechaHora);
CREATE INDEX IX_Atencion_Fecha ON dbo.Atencion (FechaHora);

CREATE TABLE dbo.Cobro
(
    AtencionID int NOT NULL CONSTRAINT PK_Cobro PRIMARY KEY,
    MedioPagoID int NOT NULL,
    Importe decimal(12,2) NOT NULL,
    CONSTRAINT FK_Cobro_Atencion FOREIGN KEY (AtencionID) REFERENCES dbo.Atencion(AtencionID),
    CONSTRAINT FK_Cobro_MedioPago FOREIGN KEY (MedioPagoID) REFERENCES dbo.MedioPago(MedioPagoID),
    CONSTRAINT CK_Cobro_Importe CHECK (Importe > 0)
);

CREATE TABLE dbo.ConsumoAtencion
(
    AtencionID int NOT NULL,
    InsumoID int NOT NULL,
    Cantidad decimal(12,2) NOT NULL,
    CONSTRAINT PK_ConsumoAtencion PRIMARY KEY (AtencionID, InsumoID),
    CONSTRAINT FK_ConsumoAtencion_Atencion FOREIGN KEY (AtencionID) REFERENCES dbo.Atencion(AtencionID),
    CONSTRAINT FK_ConsumoAtencion_Insumo FOREIGN KEY (InsumoID) REFERENCES dbo.Insumo(InsumoID),
    CONSTRAINT CK_ConsumoAtencion_Cantidad CHECK (Cantidad > 0)
);
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
        IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE DNI = @RecepcionistaDNI
                       AND Rol IN (N'Recepcionista', N'Dueño', N'Administrador') AND Activo = 1 AND Bloqueo = 0)
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

CREATE OR ALTER PROCEDURE dbo.Atencion_Propias_33ZS
    @BarberoDNI varchar(20), @Desde datetime2(0), @HastaExclusivo datetime2(0)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT a.AtencionID, a.FechaHora, s.Nombre AS Servicio,
           a.PrecioAplicado, a.PorcentajeComisionAplicado, a.ComisionImporte
    FROM dbo.Atencion a JOIN dbo.ServicioCatalogo s ON s.ServicioID = a.ServicioID
    WHERE a.BarberoDNI = @BarberoDNI AND a.FechaHora >= @Desde AND a.FechaHora < @HastaExclusivo
    ORDER BY a.FechaHora DESC, a.AtencionID DESC;
END
GO

CREATE OR ALTER PROCEDURE dbo.Atencion_Reporte_33ZS
    @Desde datetime2(0), @HastaExclusivo datetime2(0),
    @BarberoDNI varchar(20) = NULL, @ServicioID int = NULL, @MedioPagoID int = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT a.AtencionID, a.FechaHora,
           c.Apellido + N', ' + c.Nombre AS Cliente,
           u.Apellidos + N', ' + u.Nombre AS Barbero,
           s.Nombre AS Servicio, mp.Nombre AS MedioPago,
           co.Importe, a.ComisionImporte,
           COUNT(*) OVER () AS CantidadTotal,
           SUM(co.Importe) OVER () AS IngresosTotales,
           SUM(a.ComisionImporte) OVER () AS ComisionesTotales
    FROM dbo.Atencion a
    JOIN dbo.Cliente c ON c.ClienteID = a.ClienteID
    JOIN dbo.Usuario u ON u.DNI = a.BarberoDNI
    JOIN dbo.ServicioCatalogo s ON s.ServicioID = a.ServicioID
    JOIN dbo.Cobro co ON co.AtencionID = a.AtencionID
    JOIN dbo.MedioPago mp ON mp.MedioPagoID = co.MedioPagoID
    WHERE a.FechaHora >= @Desde AND a.FechaHora < @HastaExclusivo
      AND (@BarberoDNI IS NULL OR a.BarberoDNI = @BarberoDNI)
      AND (@ServicioID IS NULL OR a.ServicioID = @ServicioID)
      AND (@MedioPagoID IS NULL OR co.MedioPagoID = @MedioPagoID)
    ORDER BY a.FechaHora DESC, a.AtencionID DESC;
END
