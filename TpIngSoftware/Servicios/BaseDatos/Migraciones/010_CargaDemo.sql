SET XACT_ABORT ON;

CREATE TABLE dbo.CargaDemo
(
    Codigo varchar(50) NOT NULL CONSTRAINT PK_CargaDemo PRIMARY KEY,
    AplicadaEn datetime2(0) NOT NULL CONSTRAINT DF_CargaDemo_AplicadaEn DEFAULT (SYSDATETIME())
);
GO

CREATE OR ALTER PROCEDURE dbo.CargaDemo_Pendiente_33ZS
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CONVERT(bit, CASE WHEN EXISTS
        (SELECT 1 FROM dbo.CargaDemo WHERE Codigo = 'PresentacionInicial')
        THEN 0 ELSE 1 END);
END
GO

CREATE OR ALTER PROCEDURE dbo.CargaDemo_Confirmar_33ZS
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF EXISTS (SELECT 1 FROM dbo.CargaDemo WHERE Codigo = 'PresentacionInicial')
        THROW 51020, 'La carga de demostracion ya fue aplicada.', 1;
    INSERT INTO dbo.CargaDemo (Codigo) VALUES ('PresentacionInicial');
END
GO
