SET XACT_ABORT ON;

IF NOT EXISTS (SELECT 1 FROM dbo.Patente WHERE Nombre = N'ConsultarStock')
    INSERT INTO dbo.Patente (ID, Nombre)
    SELECT ISNULL(MAX(ID), 0) + 1, N'ConsultarStock'
    FROM dbo.Patente WITH (UPDLOCK, HOLDLOCK);

INSERT INTO dbo.Rol_Patente (RolID, PatenteID)
SELECT r.ID, p.ID
FROM dbo.Rol r CROSS JOIN dbo.Patente p
WHERE r.Nombre IN (N'Administrador', N'Dueño', N'Recepcionista')
  AND p.Nombre = N'ConsultarStock'
  AND NOT EXISTS
  (
      SELECT 1 FROM dbo.Rol_Patente rp
      WHERE rp.RolID = r.ID AND rp.PatenteID = p.ID
  );

UPDATE dbo.PN1EstadoMigracion SET DVActualizado = 0 WHERE Id = 1;
IF @@ROWCOUNT <> 1 THROW 51031, 'Falta el estado de migracion PN1.', 1;
