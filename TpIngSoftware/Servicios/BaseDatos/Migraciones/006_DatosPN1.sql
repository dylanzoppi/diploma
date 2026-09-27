SET XACT_ABORT ON;

CREATE TABLE dbo.Cliente
(
    ClienteID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Cliente PRIMARY KEY,
    Nombre nvarchar(100) NOT NULL,
    Apellido nvarchar(100) NOT NULL,
    Telefono nvarchar(30) NOT NULL,
    Correo nvarchar(150) NULL,
    FechaAlta datetime2(0) NOT NULL CONSTRAINT DF_Cliente_FechaAlta DEFAULT (SYSDATETIME()),
    Activo bit NOT NULL CONSTRAINT DF_Cliente_Activo DEFAULT (1),
    CONSTRAINT CK_Cliente_Datos CHECK
        (LEN(LTRIM(RTRIM(Nombre))) > 0 AND LEN(LTRIM(RTRIM(Apellido))) > 0
         AND LEN(LTRIM(RTRIM(Telefono))) > 0)
);
CREATE INDEX IX_Cliente_Telefono ON dbo.Cliente (Telefono);
CREATE INDEX IX_Cliente_ApellidoNombre ON dbo.Cliente (Apellido, Nombre);

CREATE TABLE dbo.BarberoPerfil
(
    UsuarioDNI varchar(20) NOT NULL CONSTRAINT PK_BarberoPerfil PRIMARY KEY,
    PorcentajeComision decimal(5,2) NOT NULL,
    Activo bit NOT NULL CONSTRAINT DF_BarberoPerfil_Activo DEFAULT (1),
    CONSTRAINT FK_BarberoPerfil_Usuario FOREIGN KEY (UsuarioDNI) REFERENCES dbo.Usuario(DNI),
    CONSTRAINT CK_BarberoPerfil_Porcentaje CHECK (PorcentajeComision >= 0 AND PorcentajeComision <= 100)
);

CREATE TABLE dbo.ServicioCatalogo
(
    ServicioID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_ServicioCatalogo PRIMARY KEY,
    Nombre nvarchar(100) NOT NULL CONSTRAINT UQ_ServicioCatalogo_Nombre UNIQUE,
    Precio decimal(12,2) NULL,
    Activo bit NOT NULL CONSTRAINT DF_ServicioCatalogo_Activo DEFAULT (1),
    CONSTRAINT CK_ServicioCatalogo_Precio CHECK (Precio IS NULL OR Precio > 0)
);

CREATE TABLE dbo.Insumo
(
    InsumoID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Insumo PRIMARY KEY,
    Codigo varchar(30) NOT NULL CONSTRAINT UQ_Insumo_Codigo UNIQUE,
    Nombre nvarchar(100) NOT NULL,
    Stock decimal(12,2) NOT NULL CONSTRAINT DF_Insumo_Stock DEFAULT (0),
    StockMinimo decimal(12,2) NOT NULL CONSTRAINT DF_Insumo_StockMinimo DEFAULT (0),
    Activo bit NOT NULL CONSTRAINT DF_Insumo_Activo DEFAULT (1),
    CONSTRAINT CK_Insumo_Stock CHECK (Stock >= 0 AND StockMinimo >= 0)
);

CREATE TABLE dbo.ServicioConsumo
(
    ServicioID int NOT NULL,
    InsumoID int NOT NULL,
    Cantidad decimal(12,2) NOT NULL,
    CONSTRAINT PK_ServicioConsumo PRIMARY KEY (ServicioID, InsumoID),
    CONSTRAINT FK_ServicioConsumo_Servicio FOREIGN KEY (ServicioID) REFERENCES dbo.ServicioCatalogo(ServicioID),
    CONSTRAINT FK_ServicioConsumo_Insumo FOREIGN KEY (InsumoID) REFERENCES dbo.Insumo(InsumoID),
    CONSTRAINT CK_ServicioConsumo_Cantidad CHECK (Cantidad > 0)
);

CREATE TABLE dbo.MedioPago
(
    MedioPagoID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_MedioPago PRIMARY KEY,
    Nombre nvarchar(50) NOT NULL CONSTRAINT UQ_MedioPago_Nombre UNIQUE,
    Activo bit NOT NULL CONSTRAINT DF_MedioPago_Activo DEFAULT (1)
);

CREATE TABLE dbo.AjusteStock
(
    AjusteID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_AjusteStock PRIMARY KEY,
    InsumoID int NOT NULL,
    UsuarioDNI varchar(20) NOT NULL,
    StockAnterior decimal(12,2) NOT NULL,
    StockNuevo decimal(12,2) NOT NULL,
    Motivo nvarchar(200) NOT NULL,
    FechaHora datetime2(0) NOT NULL CONSTRAINT DF_AjusteStock_Fecha DEFAULT (SYSDATETIME()),
    CONSTRAINT FK_AjusteStock_Insumo FOREIGN KEY (InsumoID) REFERENCES dbo.Insumo(InsumoID),
    CONSTRAINT FK_AjusteStock_Usuario FOREIGN KEY (UsuarioDNI) REFERENCES dbo.Usuario(DNI)
);

-- Son datos iniciales del catálogo, no valores fijados en C#.
-- Precio y stock quedan pendientes de la configuración del dueño.
INSERT INTO dbo.ServicioCatalogo (Nombre) VALUES (N'Corte'), (N'Barba'), (N'Corte + barba');
INSERT INTO dbo.Insumo (Codigo, Nombre) VALUES ('POMADA', N'Pomada'), ('HOJA', N'Hoja de afeitar');
INSERT INTO dbo.MedioPago (Nombre) VALUES (N'Efectivo'), (N'Transferencia'), (N'Tarjeta');
INSERT INTO dbo.ServicioConsumo (ServicioID, InsumoID, Cantidad)
SELECT s.ServicioID, i.InsumoID, 1
FROM dbo.ServicioCatalogo s JOIN dbo.Insumo i
    ON (s.Nombre = N'Corte' AND i.Codigo = 'POMADA')
    OR (s.Nombre = N'Barba' AND i.Codigo = 'HOJA')
    OR (s.Nombre = N'Corte + barba' AND i.Codigo IN ('POMADA', 'HOJA'));
GO

CREATE OR ALTER PROCEDURE dbo.Cliente_Buscar_33ZS
    @Termino nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Texto nvarchar(100) = LTRIM(RTRIM(ISNULL(@Termino, N'')));
    SELECT TOP (100) ClienteID, Nombre, Apellido, Telefono, Correo, FechaAlta
    FROM dbo.Cliente
    WHERE Activo = 1 AND
        (@Texto = N'' OR Nombre LIKE N'%' + @Texto + N'%'
         OR Apellido LIKE N'%' + @Texto + N'%'
         OR Telefono LIKE N'%' + @Texto + N'%')
    ORDER BY Apellido, Nombre, ClienteID;
END
GO

CREATE OR ALTER PROCEDURE dbo.Cliente_Registrar_33ZS
    @Nombre nvarchar(100), @Apellido nvarchar(100),
    @Telefono nvarchar(30), @Correo nvarchar(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @Nombre = LTRIM(RTRIM(@Nombre));
    SET @Apellido = LTRIM(RTRIM(@Apellido));
    SET @Telefono = LTRIM(RTRIM(@Telefono));
    SET @Correo = NULLIF(LTRIM(RTRIM(@Correo)), N'');
    IF @Nombre IS NULL OR @Nombre = N'' OR @Apellido IS NULL OR @Apellido = N''
       OR @Telefono IS NULL OR @Telefono = N''
        THROW 51001, 'Nombre, apellido y telefono son obligatorios.', 1;
    BEGIN TRY
        BEGIN TRAN;
        IF EXISTS (SELECT 1 FROM dbo.Cliente WITH (UPDLOCK, HOLDLOCK)
                   WHERE Nombre = @Nombre AND Apellido = @Apellido AND Telefono = @Telefono AND Activo = 1)
            THROW 51002, 'El cliente ya existe.', 1;
        INSERT INTO dbo.Cliente (Nombre, Apellido, Telefono, Correo)
        VALUES (@Nombre, @Apellido, @Telefono, @Correo);
        DECLARE @ID int = CONVERT(int, SCOPE_IDENTITY());
        COMMIT;
        SELECT @ID AS ClienteID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.Barbero_Listar_33ZS
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.DNI, u.Nombre, u.Apellidos, u.Activo AS UsuarioActivo,
           b.PorcentajeComision, ISNULL(b.Activo, 0) AS PerfilActivo
    FROM dbo.Usuario u LEFT JOIN dbo.BarberoPerfil b ON b.UsuarioDNI = u.DNI
    WHERE u.Rol = N'Barbero'
    ORDER BY u.Apellidos, u.Nombre;
END
GO

CREATE OR ALTER PROCEDURE dbo.Barbero_Configurar_33ZS
    @UsuarioDNI varchar(20), @PorcentajeComision decimal(5,2), @Activo bit
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @PorcentajeComision IS NULL OR @PorcentajeComision < 0 OR @PorcentajeComision > 100
        THROW 51003, 'Porcentaje de comision invalido.', 1;
    BEGIN TRY
        BEGIN TRAN;
        IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WITH (UPDLOCK, HOLDLOCK)
                       WHERE DNI = @UsuarioDNI AND Rol = N'Barbero')
            THROW 51004, 'El usuario no tiene rol Barbero.', 1;
        IF EXISTS (SELECT 1 FROM dbo.BarberoPerfil WHERE UsuarioDNI = @UsuarioDNI)
            UPDATE dbo.BarberoPerfil
            SET PorcentajeComision = @PorcentajeComision, Activo = @Activo
            WHERE UsuarioDNI = @UsuarioDNI;
        ELSE
            INSERT INTO dbo.BarberoPerfil (UsuarioDNI, PorcentajeComision, Activo)
            VALUES (@UsuarioDNI, @PorcentajeComision, @Activo);
        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.Servicio_Listar_33ZS
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ServicioID, Nombre, Precio, Activo FROM dbo.ServicioCatalogo ORDER BY ServicioID;
END
GO

CREATE OR ALTER PROCEDURE dbo.Servicio_Consumos_33ZS
    @ServicioID int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.InsumoID, i.Codigo, i.Nombre, sc.Cantidad, i.Stock
    FROM dbo.ServicioConsumo sc JOIN dbo.Insumo i ON i.InsumoID = sc.InsumoID
    WHERE sc.ServicioID = @ServicioID
    ORDER BY i.Nombre;
END
GO

CREATE OR ALTER PROCEDURE dbo.Servicio_ActualizarPrecio_33ZS
    @ServicioID int, @Precio decimal(12,2), @Activo bit
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @Precio IS NULL OR @Precio <= 0 THROW 51005, 'Precio invalido.', 1;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE dbo.ServicioCatalogo SET Precio = @Precio, Activo = @Activo
        WHERE ServicioID = @ServicioID;
        IF @@ROWCOUNT <> 1 THROW 51006, 'Servicio inexistente.', 1;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.Insumo_Listar_33ZS
AS
BEGIN
    SET NOCOUNT ON;
    SELECT InsumoID, Codigo, Nombre, Stock, StockMinimo, Activo
    FROM dbo.Insumo ORDER BY Nombre;
END
GO

CREATE OR ALTER PROCEDURE dbo.Insumo_AjustarStock_33ZS
    @InsumoID int, @StockNuevo decimal(12,2), @StockMinimo decimal(12,2),
    @UsuarioDNI varchar(20), @Motivo nvarchar(200)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @StockNuevo < 0 OR @StockMinimo < 0 OR NULLIF(LTRIM(RTRIM(@Motivo)), N'') IS NULL
        THROW 51007, 'Stock, minimo y motivo deben ser validos.', 1;
    BEGIN TRY
        BEGIN TRAN;
        DECLARE @Anterior decimal(12,2);
        SELECT @Anterior = Stock FROM dbo.Insumo WITH (UPDLOCK, HOLDLOCK) WHERE InsumoID = @InsumoID;
        IF @Anterior IS NULL THROW 51008, 'Insumo inexistente.', 1;
        UPDATE dbo.Insumo SET Stock = @StockNuevo, StockMinimo = @StockMinimo WHERE InsumoID = @InsumoID;
        INSERT INTO dbo.AjusteStock (InsumoID, UsuarioDNI, StockAnterior, StockNuevo, Motivo)
        VALUES (@InsumoID, @UsuarioDNI, @Anterior, @StockNuevo, @Motivo);
        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.MedioPago_Listar_33ZS
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MedioPagoID, Nombre FROM dbo.MedioPago WHERE Activo = 1 ORDER BY Nombre;
END
