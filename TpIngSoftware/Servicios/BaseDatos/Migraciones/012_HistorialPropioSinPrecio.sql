SET XACT_ABORT ON;
GO

CREATE OR ALTER PROCEDURE dbo.Atencion_Propias_33ZS
    @BarberoDNI varchar(20), @Desde datetime2(0), @HastaExclusivo datetime2(0)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT a.AtencionID, a.FechaHora, s.Nombre AS Servicio,
           a.PorcentajeComisionAplicado, a.ComisionImporte
    FROM dbo.Atencion a JOIN dbo.ServicioCatalogo s ON s.ServicioID = a.ServicioID
    WHERE a.BarberoDNI = @BarberoDNI AND a.FechaHora >= @Desde AND a.FechaHora < @HastaExclusivo
    ORDER BY a.FechaHora DESC, a.AtencionID DESC;
END
GO
