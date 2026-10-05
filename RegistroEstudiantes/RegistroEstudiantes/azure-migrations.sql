IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Estudiantes] (
    [EstudianteId] int NOT NULL IDENTITY,
    [Nombres] nvarchar(max) NOT NULL,
    [Direccion] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [FechaNacimiento] datetime2 NOT NULL,
    CONSTRAINT [PK_Estudiantes] PRIMARY KEY ([EstudianteId])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260920124342_Inicial', N'10.0.12');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [Libros] (
    [LibroId] int NOT NULL IDENTITY,
    [Titulo] nvarchar(max) NOT NULL,
    [Autor] nvarchar(max) NOT NULL,
    [AnoPublicacion] int NOT NULL,
    CONSTRAINT [PK_Libros] PRIMARY KEY ([LibroId])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260927165340_AgregarLibros', N'10.0.12');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Libros] ADD [Disponible] bit NOT NULL DEFAULT CAST(0 AS bit);

CREATE TABLE [Prestamos] (
    [PrestamoId] int NOT NULL IDENTITY,
    [EstudianteId] int NOT NULL,
    [LibroId] int NOT NULL,
    [FechaPrestamo] datetime2 NOT NULL,
    CONSTRAINT [PK_Prestamos] PRIMARY KEY ([PrestamoId]),
    CONSTRAINT [FK_Prestamos_Estudiantes_EstudianteId] FOREIGN KEY ([EstudianteId]) REFERENCES [Estudiantes] ([EstudianteId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Prestamos_Libros_LibroId] FOREIGN KEY ([LibroId]) REFERENCES [Libros] ([LibroId]) ON DELETE CASCADE
);

CREATE INDEX [IX_Prestamos_EstudianteId] ON [Prestamos] ([EstudianteId]);

CREATE INDEX [IX_Prestamos_LibroId] ON [Prestamos] ([LibroId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260928123411_AgregarPrestamos', N'10.0.12');

COMMIT;
GO

