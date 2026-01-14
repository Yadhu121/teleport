CREATE DATABASE teleport;
GO
USE teleport;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Files')
BEGIN
    CREATE TABLE Files (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Token NVARCHAR(50) NOT NULL UNIQUE,
        StoredName NVARCHAR(255) NOT NULL,
        OriginalName NVARCHAR(255) NOT NULL,
        IsDownloaded BIT NOT NULL DEFAULT 0,
        UploadTime DATETIME2 NOT NULL DEFAULT SYSDATETIME()
    );
END
GO

CREATE OR ALTER PROCEDURE sp_FileUpload
    @Token NVARCHAR(50),
    @StoredName NVARCHAR(255),
    @OriginalName NVARCHAR(255),
    @IsDownloaded BIT
AS
BEGIN
    INSERT INTO Files (Token, StoredName, OriginalName, IsDownloaded)
    VALUES (@Token, @StoredName, @OriginalName, @IsDownloaded);
END
GO

CREATE OR ALTER PROCEDURE sp_GetFileByToken
    @Token NVARCHAR(50)
AS
BEGIN
    SELECT TOP 1 *
    FROM Files
    WHERE Token = @Token;
END
GO

CREATE OR ALTER PROCEDURE sp_MarkDownloaded
    @Id INT
AS
BEGIN
    UPDATE Files
    SET IsDownloaded = 1
    WHERE Id = @Id;
END
GO
