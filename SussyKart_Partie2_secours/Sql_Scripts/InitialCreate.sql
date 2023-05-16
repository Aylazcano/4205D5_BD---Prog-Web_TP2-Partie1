-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--				Création de la BD
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE MASTER;
GO

IF EXISTS(SELECT * FROM sys.databases WHERE name='TP2_SussyKart')
BEGIN
    DROP DATABASE TP2_SussyKart
END
CREATE DATABASE TP2_SussyKart
GO

-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--		Configurer FILESTREAM avec SQL Server
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart
GO

EXEC sp_configure filestream_access_level, 2 RECONFIGURE

ALTER DATABASE TP2_SussyKart
ADD FILEGROUP FG_Images CONTAINS FILESTREAM;
GO
ALTER DATABASE TP2_SussyKart
ADD FILE (
	NAME = FG_Images,
	FILENAME = 'C:\EspaceLabo'
)
TO FILEGROUP FG_Images
GO

-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--	Préparer une table qui stockera des images
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

-- Images.Image ou Utilisateurs.Image???
CREATE TABLE Images.Image(
	ImageID int IDENTITY(1,1),
	Nom nvarchar(100) NOT NULL,
	identifiant uniqueidentifier NOT NULL ROWGUIDCOL,
	CONSTRAINT PK_Image_ImageID PRIMARY KEY (ImageID)
);
GO

ALTER TABLE Images.Image 
ADD CONSTRAINT UC_Image_Identifiant UNIQUE (Identifiant);
GO

ALTER TABLE Images.Image 
ADD CONSTRAINT DF_Image_Identifiant DEFAULT newid() FOR Identifiant;
GO

ALTER TABLE Images.Image 
ADD FichierImage varbinary(max) FILESTREAM NULL;
GO

