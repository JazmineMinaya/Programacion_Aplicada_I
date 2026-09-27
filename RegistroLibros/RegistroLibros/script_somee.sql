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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913083210_Inicial'
)
BEGIN
    CREATE TABLE [Libros] (
        [LibroId] int NOT NULL IDENTITY,
        [Titulo] nvarchar(max) NOT NULL,
        [Autor] nvarchar(max) NOT NULL,
        [AnoPublicacion] int NOT NULL,
        CONSTRAINT [PK_Libros] PRIMARY KEY ([LibroId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913083210_Inicial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260913083210_Inicial', N'10.0.12');
END;

COMMIT;
GO

