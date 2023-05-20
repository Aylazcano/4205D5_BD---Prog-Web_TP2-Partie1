-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--				Création des tables
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

CREATE TABLE Utilisateurs.Amitie(
	AmitieID int IDENTITY(1,1),
	UtilisateurID int NOT NULL,
	UtilisateurID_Ami int NOT NULL,
	CONSTRAINT PK_Amitie_AmitieID PRIMARY KEY (AmitieID)
);


-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--			  Création des contraintes
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

ALTER TABLE Utilisateurs.Amitie 
ADD CONSTRAINT FK_Amitie_UtilisateurID
FOREIGN KEY (UtilisateurID) REFERENCES Utilisateurs.Utilisateur(UtilisateurID)
---- Inutile avec le nouveau déclencheur INSTEAD OF DELETE Utilisateurs.Utilisateur
--ON DELETE CASCADE
--ON UPDATE CASCADE;
GO

ALTER TABLE Utilisateurs.Amitie 
ADD CONSTRAINT FK_Amitie_UtilisateurID_Ami
FOREIGN KEY (UtilisateurID_Ami) REFERENCES Utilisateurs.Utilisateur(UtilisateurID);
GO

-- Utilisateur avec amis uniques
ALTER TABLE Utilisateurs.Amitie
ADD CONSTRAINT UC_Amitie_AmiUnique 
UNIQUE (UtilisateurID,  UtilisateurID_Ami);
GO

-- Empeche de s'ajouter en tant qu'ami
ALTER TABLE Utilisateurs.Amitie
ADD CONSTRAINT CK_Amitie_SajouterSoiMeme
CHECK (UtilisateurID <> UtilisateurID_Ami);
GO