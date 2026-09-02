USE [TpIngSoftware];
GO

/*
   Carga: Patentes, Familias (ejemplo), Roles, Usuarios
   ===================================================================== */

/* ---------- 1. Limpiar datos existentes---------- */
DELETE FROM Rol_Patente;
DELETE FROM Rol_Familia;
DELETE FROM Patente_Familia;
DELETE FROM FamiliaN;
DELETE FROM DV;
DELETE FROM Evento;
DELETE FROM Usuario;
DELETE FROM Rol;
DELETE FROM Familia;
DELETE FROM Patente;
GO

/* ---------- 2. Patentes (catálogo de permisos) ---------- */
INSERT INTO Patente (ID, Nombre) VALUES
(1, 'AltaUsuario'),
(2, 'BajaUsuario'),
(3, 'ModificacionUsuario'),
(4, 'DesbloquearUsuario'),
(5, 'AltaPerfil'),
(6, 'BajaPerfil'),
(7, 'ModificacionPerfil'),
(8, 'CambiarClave'),
(9, 'ConsultarBitacora'),
(10, 'GestionRespaldos');
GO

/* ---------- 3. Familia de ejemplo ----------*/
INSERT INTO Familia (ID, Nombre) VALUES
(1, 'GestionDeUsuarios');

INSERT INTO Patente_Familia (PatenteID, FamiliaID) VALUES
(1, 1),   -- AltaUsuario
(2, 1),   -- BajaUsuario
(3, 1),   -- ModificacionUsuario
(4, 1);   -- DesbloquearUsuario
GO

/* ---------- 4. Roles ---------- */
INSERT INTO Rol (ID, Nombre) VALUES
(1, 'Administrador'),
(2, 'Invitado');
GO

/* Administrador: TODAS las patentes (asignadas directo) */
INSERT INTO Rol_Patente (RolID, PatenteID)
SELECT 1, ID FROM Patente;

/* Invitado: solo puede cambiar su clave */
INSERT INTO Rol_Patente (RolID, PatenteID)
SELECT 2, ID FROM Patente WHERE Nombre = 'CambiarClave';
GO

/* ---------- 5. Usuarios --------- */

-- Admin:  email = admin@admin.com   |  password = Admin123
INSERT INTO Usuario (DNI, Apellidos, Nombre, Login, Password, Rol, Email, Bloqueo, Activo, Idioma)
VALUES (
  '10000000', 'Administrador', 'Admin',
  'admin@admin.com',
  LOWER(CONVERT(varchar(64), HASHBYTES('SHA2_256', CONVERT(varbinary(255), 'Admin123')), 2)),
  'Administrador', 'admin@admin.com', 0, 1, 'ESP'
);

-- Invitado:  email = invitado@invitado.com  |  password = Invitado123
INSERT INTO Usuario (DNI, Apellidos, Nombre, Login, Password, Rol, Email, Bloqueo, Activo, Idioma)
VALUES (
  '20000000', 'Invitado', 'Invitado',
  'invitado@invitado.com',
  LOWER(CONVERT(varchar(64), HASHBYTES('SHA2_256', CONVERT(varbinary(255), 'Invitado123')), 2)),
  'Invitado', 'invitado@invitado.com', 0, 1, 'ESP'
);
GO

/* ---------- 6. Verificación ---------- */
SELECT 'Patentes' AS Tabla, COUNT(*) AS Filas FROM Patente
UNION ALL SELECT 'Familias', COUNT(*) FROM Familia
UNION ALL SELECT 'Roles', COUNT(*) FROM Rol
UNION ALL SELECT 'Usuarios', COUNT(*) FROM Usuario;

SELECT u.Email, u.Rol, COUNT(rp.PatenteID) AS CantPatentes
FROM Usuario u
JOIN Rol r ON r.Nombre = u.Rol
LEFT JOIN Rol_Patente rp ON rp.RolID = r.ID
GROUP BY u.Email, u.Rol;
GO
