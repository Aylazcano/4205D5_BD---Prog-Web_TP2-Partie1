-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--	Création de la table Avatar qui stockera des images (Avatar)
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

CREATE TABLE Utilisateurs.Avatar(
	AvatarID int IDENTITY(1,1),
	UtilisateurID int NOT NULL,
	Identifiant uniqueidentifier NOT NULL ROWGUIDCOL,
	CONSTRAINT PK_Avatar_AvatarID PRIMARY KEY (AvatarID)
);
GO

ALTER TABLE Utilisateurs.Avatar 
ADD CONSTRAINT UC_Avatar_Identifiant UNIQUE (Identifiant);
GO

ALTER TABLE Utilisateurs.Avatar 
ADD CONSTRAINT DF_Avatar_Identifiant DEFAULT newid() FOR Identifiant;
GO

ALTER TABLE Utilisateurs.Avatar 
ADD FichierAvatar varbinary(max) FILESTREAM NULL;
GO