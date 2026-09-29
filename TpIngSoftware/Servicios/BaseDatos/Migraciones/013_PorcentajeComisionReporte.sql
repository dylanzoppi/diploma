SET XACT_ABORT ON;
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
           co.Importe, a.PorcentajeComisionAplicado, a.ComisionImporte,
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
GO
