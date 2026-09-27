SET XACT_ABORT ON;

IF EXISTS
(
    SELECT 1
    FROM sys.columns columna
    INNER JOIN sys.types tipo ON tipo.user_type_id = columna.user_type_id
    WHERE columna.object_id = OBJECT_ID(N'dbo.Usuario')
      AND columna.name = N'Rol'
      AND (tipo.name <> N'nvarchar' OR columna.max_length <> 200)
)
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM dbo.Usuario
        WHERE LEN(Rol) > 100
    )
        THROW 51001, 'No se puede ampliar Usuario.Rol: existe un valor mayor a 100 caracteres.', 1;

    ALTER TABLE dbo.Usuario ALTER COLUMN Rol nvarchar(100) NOT NULL;
END;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.key_constraints
    WHERE parent_object_id = OBJECT_ID(N'dbo.Rol')
      AND name = N'UQ_Rol_Nombre'
)
BEGIN
    IF EXISTS
    (
        SELECT Nombre
        FROM dbo.Rol
        GROUP BY Nombre
        HAVING COUNT(*) > 1
    )
        THROW 51002, 'No se puede crear UQ_Rol_Nombre: existen roles duplicados.', 1;

    ALTER TABLE dbo.Rol ADD CONSTRAINT UQ_Rol_Nombre UNIQUE (Nombre);
END;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'dbo.Usuario')
      AND name = N'FK_Usuario_Rol'
)
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM dbo.Usuario usuario
        LEFT JOIN dbo.Rol rol ON rol.Nombre = usuario.Rol
        WHERE rol.ID IS NULL
    )
        THROW 51003, 'No se puede crear FK_Usuario_Rol: existen usuarios con roles inexistentes.', 1;

    ALTER TABLE dbo.Usuario WITH CHECK
        ADD CONSTRAINT FK_Usuario_Rol FOREIGN KEY (Rol)
        REFERENCES dbo.Rol (Nombre)
        ON UPDATE CASCADE;
END;
